using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using PortalFibrobeton;
using System.Threading;
using System.Runtime.Remoting.Channels;
using System.Timers;
using Timer = System.Timers.Timer;
using Timer2 = System.Threading.Timer;
using PortalFibrobeton.Models.Entity;
using System.Collections.Generic;
using PortalFibrobeton.Models.Class.ARGE.OlgunlukCihazClass;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

public class MqttService
{
    private MqttClient _client;
    private readonly IHubContext _hubContext;

    //Culture Info
    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

    //Ortam Sıcaklık
    public string ortamSicaklik;

    //Mqtt Dinleme
    string sensorSicaklik;

    //Min. Sıcaklık
    double minTemp = 0.0;

    //Cihaz Olgunluk
    Dictionary<string, double> olgunlukToplamList = new Dictionary<string, double>();

    Dictionary<string, OlgunlukCihazClass> deviceStates = new Dictionary<string, OlgunlukCihazClass>();
    
    //Cihaz Sıcaklıkları
    Dictionary<string, List<double>> sicaklikList = new Dictionary<string, List<double>>();
    private Timer sicaklikTimer;
    private Timer testDurumTimer;

    OlgunlukEntities dbOlg = new OlgunlukEntities();

    public MqttService()
    {

        _hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        TestDurumuGuncelle();

        string mqttBrokerAdress = "193.100.100.105";
        _client = new MqttClient(mqttBrokerAdress);

        string clientId = Guid.NewGuid().ToString();
        _client.Connect(clientId);

        _client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        string[] topics = { "Olgunluk1_pub","Olgunluk1_durum","Olgunluk1_sure","Olgunluk1_rstart", "sensor/temperature", 
                            "Olgunluk2_pub","Olgunluk2_durum","Olgunluk2_sure","Olgunluk2_rstart", "sefaguntepe",
                            "Olgunluk3_pub","Olgunluk3_durum","Olgunluk3_sure","Olgunluk3_rstart",
        "Olgunluk4_pub","Olgunluk4_durum","Olgunluk4_sure","Olgunluk4_rstart",
        "Olgunluk5_pub","Olgunluk5_durum","Olgunluk5_sure","Olgunluk5_rstart",
        "Olgunluk6_pub","Olgunluk6_durum","Olgunluk6_sure","Olgunluk6_rstart",
        "Olgunluk7_pub","Olgunluk7_durum","Olgunluk7_sure","Olgunluk7_rstart",
        "Olgunluk8_pub","Olgunluk8_durum","Olgunluk8_sure","Olgunluk8_rstart",
        "Olgunluk9_pub","Olgunluk9_durum","Olgunluk9_sure","Olgunluk9_rstart",
        "Olgunluk10_pub","Olgunluk10_durum","Olgunluk10_sure","Olgunluk10_rstart",};
        byte qosLevel = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;

        foreach(string topic in topics)
        {
            _client.Subscribe(new string[] { topic }, new byte[] { qosLevel });
        }

        

        //Sicaklik Ortalama Timer
        sicaklikTimer = new Timer(600000);
        sicaklikTimer.Elapsed += SicaklikOrtalamaHesap;
        sicaklikTimer.AutoReset = true;
        sicaklikTimer.Start();

        //Test Durumu Güncelle
        testDurumTimer = new Timer(30000);
        testDurumTimer.Elapsed += (sender, e) => TestDurumuGuncelle();
        testDurumTimer.AutoReset = true;
        testDurumTimer.Start();
        

    }

    //Veritabanına Veri Kaydetme
    public void SaveElapsedTimeToDatabase(TimeSpan elapsedTime, string topic, double sicaklik, double olgunluk, double basinc, double egilme)
    {
        using(OlgunlukEntities dbOlgN = new OlgunlukEntities())
        {
            var queryTest = dbOlg.OlgunlukCihazi.ToList();
            double ortamsicaklikDouble = 0.0;

            if (ortamSicaklik != null)
            {
                ortamsicaklikDouble = double.Parse(ortamSicaklik, culture);
            }
            else
            {
                ortamsicaklikDouble = 0.0;
            }


            var sicaklikSensor = new OlgunlukSensor
            {
                test_sure = (int?)elapsedTime.TotalMilliseconds,
                sensor_tarih = DateTime.Now,
                cihaz_no = topic.ToString(),
                testID = queryTest.Where(a => a.cihaz_no == topic).Select(a => a.ID).Last(),
                sensor_sicaklik = sicaklik,
                ortam_sicaklik = ortamsicaklikDouble,
                olgunluk = olgunluk,
                basinc_dayanimi = basinc,
                egilme_dayanimi = egilme


            };

            dbOlg.OlgunlukSensor.Add(sicaklikSensor);
            dbOlg.SaveChanges();
        }
        
    }


    //Timer Sayacı ++
    private void UpdateTimer(string topic)
    {
        if (deviceStates.ContainsKey(topic))
        {
            var state = deviceStates[topic];
            //if (DateTime.Now - state.LastDataReceivedTime > TimeSpan.FromMinutes(2))
            //{
            //    state.Timer.Stop();
            //    _hubContext.Clients.All.Timeout(topic);
            //    return;
            //}
            state.ElapsedTime = state.ElapsedTime.Add(TimeSpan.FromSeconds(1));

            state.TahminiKalanSure -= 1;  //tahmini gevşetme süresini azalt..

           
            _hubContext.Clients.All.updateTimer(state.ElapsedTime.ToString(@"hh\:mm\:ss"), state.TahminiKalanSure.ToString(@"hh\:mm\:ss"));
        }
    }

    //Zamanlayıcıyı Başlat
    private void StartTimer(string topic)
    {

        if (!deviceStates.ContainsKey(topic) || deviceStates[topic].Timer == null)
        {
            deviceStates[topic] = new OlgunlukCihazClass
            {
                ElapsedTime = TimeSpan.Zero,
                LastDataReceivedTime = DateTime.Now,
                Timer = new Timer(1000)
            };

            deviceStates[topic].Timer.Elapsed += (sender, e) => UpdateTimer(topic);
            deviceStates[topic].Timer.AutoReset = true;
        }

        if (deviceStates[topic].Timer != null)
        {
            deviceStates[topic].Timer.Start();
        }
        
    }

    //Yeni Test Başlayınca Zamanlayıcıyı Sıfırla
    public void ResetTimer(string topic)
    {
        if (deviceStates.ContainsKey(topic))
        {
            deviceStates[topic].ElapsedTime = TimeSpan.Zero;
            deviceStates[topic].LastDataReceivedTime = DateTime.Now;
        }

        if (olgunlukToplamList.ContainsKey(topic))
        {
            olgunlukToplamList[topic] = 0.0;
        }
    }

    //Test Durumunu Kontrol Et
    void TestDurumuGuncelle()
    {
        using (var dbOlgN = new OlgunlukEntities())
        {
            var cihazlar = dbOlgN.OlgunlukCihazi.ToList();
            foreach (var device in cihazlar)
            {
                if (!deviceStates.ContainsKey(device.cihaz_no))
                {
                    deviceStates[device.cihaz_no] = new OlgunlukCihazClass();
                }

                var sonTestDurum = dbOlgN.OlgunlukCihazi
                    .Where(a => a.cihaz_no == device.cihaz_no)
                    .OrderByDescending(a => a.test_tarihi)
                    .Select(a => a.test_durum)
                    .FirstOrDefault();

                if (sonTestDurum.HasValue)
                {
                    deviceStates[device.cihaz_no].TestDurum = sonTestDurum.Value;
                }
            }
        }
    }

    //Mqtt Veri Okuma
    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {

        //Sıcaklık Verileri Pub
        if (e.Topic.EndsWith("_pub"))
        {
            //Sensor Verisi
            sensorSicaklik = Encoding.UTF8.GetString(e.Message);
         

            if (deviceStates.ContainsKey(e.Topic) && deviceStates[e.Topic].TestDurum == true)
            {
                if (deviceStates[e.Topic].Timer == null || deviceStates[e.Topic].ElapsedTime < TimeSpan.FromMinutes(2))
                {
                    StartTimer(e.Topic);
                }

                deviceStates[e.Topic].LastDataReceivedTime = DateTime.Now;
            }



            //Sıcaklıkların Ortalaması İçin Listeye Kaydet
            if (double.TryParse(sensorSicaklik, out double sicakliklar))
            {
                if (!sicaklikList.ContainsKey(e.Topic))
                {
                    sicaklikList[e.Topic] = new List<double>();
                }

                sicaklikList[e.Topic].Add(sicakliklar / 100);
            }
        }


        //Ortam Sıcaklık
        if (e.Topic == "sensor/temperature")
        {
            ortamSicaklik = Encoding.UTF8.GetString(e.Message);

            _hubContext.Clients.All.deneme(ortamSicaklik);

        }
    }

    //Sıcaklık Ortalama Hesapla
    private void SicaklikOrtalamaHesap(object sender, ElapsedEventArgs e)
    {
        double olg = 0;
        double c = 0.0;
        double zaman = 10.0 / 60.0;
        double lnc = 0.0;
        double lnOlg = 0.0;
        double basincDayanim = 0.0;
        double egilmeDayanim = 0.0;


        foreach (var topic in sicaklikList.Keys)
        {

            var durumCheckQuery = dbOlg.OlgunlukCihazi.Where(a => a.cihaz_no == topic).Select(a => a.test_durum).ToList();
            var lastdurumCheck = durumCheckQuery.LastOrDefault();

            if (sicaklikList[topic].Count > 0 && lastdurumCheck != null && lastdurumCheck == true)
            {
                double ortalamaSicaklik = sicaklikList[topic].Average();
                ortalamaSicaklik = Math.Round(ortalamaSicaklik, 2);

                sicaklikList[topic].Clear();

                if (!olgunlukToplamList.ContainsKey(topic))
                {
                    olgunlukToplamList[topic] = 0.0;
                }

                var queryCihaz = dbOlg.OlgunlukCihazi.Where(a => a.cihaz_no == topic).ToList();
                var testID = queryCihaz.Select(a => a.ID).Last();
                var altLimitOlg = queryCihaz.Select(a => a.olgunluk_Baslangic).Last();
                var ustLimitOlg = queryCihaz.Select(a => a.olgunluk_Bitis).Last();
                var hammaddeTipi = queryCihaz.Select(a => a.hammadde_Tipi).Last();
                var durumTopic = topic.Replace("_pub", "_durum");
                var olgunlukTopic = topic.Replace("_pub", "_olgunluk");
                var sureTopic = topic.Replace("_pub", "_sure");


                //Olgunluk Hesaplama
                if (hammaddeTipi == "NAW3")
                {
                    //Olgunluk
                    c = 1.1;
                    lnc = (double)Math.Log(c);
                    olg = ((Math.Pow(c, (0.1 * ortalamaSicaklik - 1.245)) - Math.Pow(c, -2.245)) * 10.0 / lnc) * zaman;
                    olgunlukToplamList[topic] += olg;

                    //Basınç Dayanım
                    lnOlg = (double)Math.Log(olg);
                    basincDayanim = (double)((36.529 * lnOlg) - 204.69);

                    //Eğilme Dayanım
                    egilmeDayanim = basincDayanim + 5;


                    //Gevşetme Uyarı
                    if (olgunlukToplamList[topic] < altLimitOlg)
                    {
                        PublishData(durumTopic, "Bekle");
                    }
                    if (olgunlukToplamList[topic] >= altLimitOlg && olgunlukToplamList[topic] <= ustLimitOlg)
                    {
                        PublishData(durumTopic, "Gevset");
                        _hubContext.Clients.All.Gevset(topic);
                    }
                    if (olgunlukToplamList[topic] > ustLimitOlg)
                    {
                        PublishData(durumTopic, "Tamamlandi");
                        _hubContext.Clients.All.Gevset(topic);
                    }

                }
                if (hammaddeTipi == "B3SP")
                {
                    //Olgunluk
                    c = 1.2;
                    lnc = (double)Math.Log(c);
                    olg = ((Math.Pow(c, (0.1 * ortalamaSicaklik - 1.245)) - Math.Pow(c, -2.245)) * 10.0 / lnc) * zaman;
                    olgunlukToplamList[topic] += olg;

                    //Basınç Dayanım
                    lnOlg = (double)Math.Log(olg);
                    basincDayanim = (double)((27.787 * lnOlg) - 111.57);

                    //Eğilme Dayanım
                    egilmeDayanim = basincDayanim + 5;

                    //Gevşetme Uyarı
                    if (olgunlukToplamList[topic] < altLimitOlg)
                    {
                        PublishData(durumTopic, "Bekle");
                    }
                    if (olgunlukToplamList[topic] >= altLimitOlg && olgunlukToplamList[topic] <= ustLimitOlg)
                    {
                        PublishData(durumTopic, "Gevset");
                    }
                    if (olgunlukToplamList[topic] > ustLimitOlg)
                    {
                        PublishData(durumTopic, "Tamamlandi");
                    }

                }


                //Geçen zaman
                TimeSpan elapsedTime = deviceStates[topic].ElapsedTime;

                //Publish Süre
                PublishData(sureTopic, elapsedTime.ToString(@"hh\:mm\:ss"));

                //Publish Olgunluk
                PublishData(olgunlukTopic, Math.Round(olgunlukToplamList[topic], 2).ToString());
             

                //Gevşetme Zamanına Kalan Tahmini                
                var fark = altLimitOlg - olgunlukToplamList[topic];
                var tahminiGevşetmeZaman = fark / olg;
                var tahminiGevşetmeSure = zaman * 10;
                deviceStates[topic].TahminiKalanSure = tahminiGevşetmeSure;
                

                //DB'ye veri kayıt
                SaveElapsedTimeToDatabase(elapsedTime, topic, ortalamaSicaklik, olgunlukToplamList[topic], basincDayanim, egilmeDayanim);

                //En düşük sıcaklık
                if (ortalamaSicaklik > 5)
                {
                    if (ortalamaSicaklik < deviceStates[topic].MinTemp)
                    {
                        deviceStates[topic].MinTemp = ortalamaSicaklik;
                    }
                    else
                    {
                        var querySensor = dbOlg.OlgunlukSensor.Where(a => a.cihaz_no == topic).ToList();
                        var lastQuery = querySensor.Where(a => a.testID == testID).ToList();
                        deviceStates[topic].MinTemp = (double)lastQuery.Select(a => a.sensor_sicaklik).Min();
                    }
                }

                var olgunlukViewPage = (olgunlukToplamList[topic] / altLimitOlg) * 100;
                

                //Mqtt to View

                
                //Grafik Verileri ve Gevşetme
                _hubContext.Clients.All.totalOlg(topic, olgunlukViewPage, tahminiGevşetmeSure);
                //Oralama Sıcaklık
                _hubContext.Clients.All.averageTemp(topic, ortalamaSicaklik);
                //Toplam Olgunluk Text
                _hubContext.Clients.All.olg(topic, Math.Round(olgunlukToplamList[topic],2));
                //Min. Sıcaklık Verisi
                _hubContext.Clients.All.minTemp(topic, deviceStates[topic].MinTemp);
                
            }
        }

    }

    public void GevsetmeZamaniTahmini(int? altLimit,double kumulatifToplam , double sonOlg,string topic)
    {
        var fark = altLimit - kumulatifToplam;
        var zaman = fark / sonOlg;
        var dakika = zaman * 10;

        _hubContext.Clients.All.TahminiKalan(dakika, topic);
    }

    //Mqtt Veri Gönderme
    public void PublishData(string topic, string payload)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(payload);
        byte qosLevel = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
        bool retain = true;

        _client.Publish(topic, messageBytes, qosLevel, retain);
    }

    //Sayaçtaki veriyi okuma
    public TimeSpan GetCurrentTimeElapsed(string topic)
    {
        return deviceStates[topic].ElapsedTime;
    }

}