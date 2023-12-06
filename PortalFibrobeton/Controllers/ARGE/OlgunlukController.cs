using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortalFibrobeton.Attributelar;
using PortalFibrobeton.Models.Class.ARGE.OlgunlukCihazClass;
using PortalFibrobeton.Models.Class.ARGE.OlgunlukViewModels;
using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace PortalFibrobeton.Controllers.ARGE
{
    
    public class OlgunlukController : Controller
    {
        PERA_FIBROEntities db1 = new PERA_FIBROEntities();
        OlgunlukEntities dbOlgunluk = new OlgunlukEntities();
        MATURITYEntities dbOlgunlukEski = new MATURITYEntities();
        string pozBark;
        DateTime today = DateTime.Today;

        //MqttService
        private MqttService _mqttService;
        

        // GET: Olgunluk

        public OlgunlukController()
        {
            _mqttService = MvcApplication.MqttServiceInstance;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult HomePageOlgunluk()
        {
            //var today = DateTime.Today.DayOfWeek.ToString("dddd", System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            string bugun = today.ToString("dddd", System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            string ay = today.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            string yil = today.ToString("yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            string hafta = today.Day.ToString();
            ViewBag.today = bugun;
            ViewBag.hafta = hafta;
            ViewBag.month = ay;
            ViewBag.year = yil;


            //Bugün Veri
            var queryDay = dbOlgunlukEski.mat.SqlQuery("SELECT * FROM mat where tarih = DATEADD(day, datediff(day, 0 , GETDATE()),0)").ToList();
            ViewBag.todayCount = queryDay.Select(a => a.sensor_adi).Distinct().Count();

            //Haftalık Veri
            var queryHafta = dbOlgunlukEski.mat.SqlQuery("SELECT * FROM mat where DATEPART(week,tarih) = DATEPART(week,GETDATE()) AND DATEPART(yyyy,tarih) = datepart(yyyy,GETDATE())").ToList();
            ViewBag.weekCount = queryHafta.Select(a => a.sensor_adi).Distinct().Count();
                
            //Aylık Veri
            var queryMonth = dbOlgunlukEski.mat.SqlQuery("SELECT * FROM mat where DATEPART(MONTH,tarih) = DATEPART(MONTH,GETDATE()) AND DATEPART(yyyy,tarih) = datepart(yyyy,GETDATE())").ToList();
            ViewBag.monthCount = queryMonth.Select(a => a.sensor_adi).Distinct().Count();

            //
            //SELECT DISTINCT sensor_adi FROM mat where DATEPART(MONTH,tarih) = DATEPART(MONTH,GETDATE()) and datepart(yyyy,tarih) = datepart(yyyy,GETDATE())


            //Olgunluk Dashboard

            var uniqeCihazNoList = dbOlgunluk.OlgunlukCihazi.Select(a => a.cihaz_no).Distinct().ToList();

            var sonOlgunlukCihazList = new List<OlgunlukCihazi>();
            foreach(var cihazNo in uniqeCihazNoList)
            {
                var sonOlgunlukCihaz = dbOlgunluk.OlgunlukCihazi.Where(a => a.cihaz_no == cihazNo).OrderByDescending(a => a.test_tarihi).FirstOrDefault();

                if(sonOlgunlukCihaz != null && sonOlgunlukCihaz.test_durum == true)
                {
                    sonOlgunlukCihazList.Add(sonOlgunlukCihaz);
                }
            }

            var viewModel = new OlgunlukViewModelDashboard
            {
                OlgunlukCihazList = sonOlgunlukCihazList,
                OlgunlukSensorList = dbOlgunluk.OlgunlukSensor.ToList()
            };


            //Olgunluk verileri

            //Güncel 
            return View(viewModel);


        }

        [HttpPost]
        public ActionResult HomePageOlgunluk(OlgunlukCihazi p1, string cihazNo)
        {
            var viewModel = new OlgunlukViewModelDashboard
            {
                OlgunlukCihazList = dbOlgunluk.OlgunlukCihazi.ToList(),
                OlgunlukSensorList = dbOlgunluk.OlgunlukSensor.ToList()
            };

            var query = dbOlgunluk.OlgunlukCihazi.Where(a => a.cihaz_no == cihazNo).ToList();
            if(query.Count > 0)
            {
                p1.olgunluk_Baslangic = query.Select(a => a.olgunluk_Baslangic).Last();
                p1.olgunluk_Bitis = query.Select(a => a.olgunluk_Bitis).Last();
                p1.kalip_Turu = query.Select(a => a.kalip_Turu).Last();
                p1.extra_Sure = query.Select(a => a.extra_Sure).Last();
                p1.baslangic_Sicaklik = query.Select(a => a.baslangic_Sicaklik).Last();
                p1.hammadde_Tipi = query.Select(a => a.hammadde_Tipi).Last();
                p1.notDurum = query.Select(a => a.notDurum).Last();
                p1.projeAdi = query.Select(a => a.projeAdi).Last();
                p1.poz = query.Select(a => a.poz).Last();
                p1.cihaz_no = query.Select(a => a.cihaz_no).Last();
                p1.testi_baslatan = query.Select(a => a.testi_baslatan).Last();
                p1.test_tarihi = DateTime.Now;
                p1.test_durum = false;

                dbOlgunluk.OlgunlukCihazi.Add(p1);
                dbOlgunluk.SaveChanges();
            }




            return RedirectToAction("HomePageOlgunluk", "Olgunluk");
        }


        [HttpGet]
        public ActionResult AddTest()
        {
            //Olgunluk Cihazları Kontrol
            var cihazlar = dbOlgunluk.OlgunlukCihazi.GroupBy(c => c.cihaz_no)
                .Select(g => g.OrderByDescending(c => c.test_tarihi).FirstOrDefault()).Where(a => a.test_durum == false).Select(b => b.cihaz_no).ToList();

            ViewBag.cihazlar = cihazlar;
            return View();
        }

        [HttpPost]
        public ActionResult AddTest(string pb)
        {
            try
            {
                ViewBag.pozBarkod = pb;
                //Poz
                string pozSQL = "SELECT * From KALIP_IS_EMRI_POZ where POZ_BARKOD =@p1";
                SqlParameter pozS1 = new SqlParameter("@p1", pb);
                object[] param = new object[] { pozS1 };
                var queryPOZ = db1.KALIP_IS_EMRI_POZ.SqlQuery(pozSQL, param).ToList();
                var pozID = queryPOZ.Select(a => a.POZ_ID).LastOrDefault();
                ViewBag.pozID = pozID;
                var pozNO = queryPOZ.Select(a => a.POZ_NO).LastOrDefault();
                ViewBag.pozNo = pozNO;
                var pozBarkodNo = queryPOZ.Select(a => a.POZ_BARKOD).LastOrDefault();
                ViewBag.pozBarkodNo = pozBarkodNo;
                var kalipIsId = queryPOZ.Select(a => a.KALIP_IS_ID).LastOrDefault();
                var siparisID = queryPOZ.Select(a => a.SIPARIS_ID).LastOrDefault();


                //İlk Pozu Yakalama
                string projeSQLFirst = "SELECT * FROM KALIP_IS_EMRI_POZ WHERE KALIP_IS_ID = @x1 AND SIRA = '1'";
                SqlParameter projeF1 = new SqlParameter("@x1", kalipIsId);
                object[] paramF1 = new object[] { projeF1 };
                var queryfirst = db1.KALIP_IS_EMRI_POZ.SqlQuery(projeSQLFirst, paramF1).ToList();
                var firstPoz = queryfirst.Select(a => a.POZ_NO).LastOrDefault();

                //Malzeme

                string malzemeSQL = "SELECT * From KALIP_IS_EMRI_KALIP_OLUSTUR where ISLEM_TURU = 'Kalıp' AND KALIP_IS_ID = @id";
                SqlParameter malzemeP1 = new SqlParameter("@id", kalipIsId);
                object[] param3 = new object[] { malzemeP1 };
                var queryMalzeme = db1.KALIP_IS_EMRI_KALIP_OLUSTUR.SqlQuery(malzemeSQL, param3).ToList();
                var malzeme = queryMalzeme.Select(a => a.ISLEM).FirstOrDefault();
                ViewBag.malzeme = malzeme;

                //Proje
                string projeSQL = "SELECT * FROM KALIP_IS_EMRI where POZ_NO = @p2";
                SqlParameter projeS1 = new SqlParameter("@p2", firstPoz);
                object[] param2 = new object[] { projeS1 };
                var queryProje = db1.KALIP_IS_EMRI.SqlQuery(projeSQL, param2).ToList();
                var proje = queryProje.Select(a => a.ANA_POZ_PROJE_ADI).LastOrDefault();
                ViewBag.proje = proje;
                var kalıpM2 = queryProje.Select(a => a.KALIP_M2).LastOrDefault();
                ViewBag.kalıpM2 = kalıpM2;
                var barkod = queryProje.Select(a => a.IS_EMRI_BARKOD).LastOrDefault();
                ViewBag.barkod = barkod;
                var id = queryProje.Select(a => a.ID).LastOrDefault();

                //Olgunluk Cihazları Kontrol
                var cihazlar = dbOlgunluk.OlgunlukCihazi.GroupBy(c => c.cihaz_no)
                    .Select(g => g.OrderByDescending(c => c.test_tarihi).FirstOrDefault()).Where(a => a.test_durum == false).Select(b => b.cihaz_no).ToList();

                
                ViewBag.cihazlar = cihazlar;
                return View();
            }

            catch (SqlException)
            {
                TempData["errorQuery"] = "Poz barkod numarası eksik veya hatalı!";
            }

            return RedirectToAction("HomePageOlgunluk", "Olgunluk");
        }

        [HttpPost]
        public ActionResult SaveTest(OlgunlukCihazi p1)
        {
            _mqttService.ResetTimer(p1.cihaz_no);
            dbOlgunluk.OlgunlukCihazi.Add(p1);
            p1.pozBarkod = pozBark;
            p1.test_tarihi = DateTime.Now;
            p1.test_durum = true;
            p1.testi_baslatan = Session["UserName"].ToString();
            dbOlgunluk.SaveChanges();
            TempData["successAdd"] = "Test kaydı başarıyla oluşturuldu!";

            return RedirectToAction("HomePageOlgunluk", "Olgunluk");
        }


        [HttpGet]
        public ActionResult OlgunlukRaporlari()
        {

            var emptyModel = new OlgunlukViewModelDashboard
            {
                OlgunlukCihazList = new List<OlgunlukCihazi>(),
                SensorCihazList = new List<OlgunlukViewModelDashboard>()
            };
            return View(emptyModel);
        }

        [HttpPost]
        public ActionResult OlgunlukRaporlari(DateTime bas, DateTime bit)
        {
            //İlk tablo için sorgu
            var queryCihaz = dbOlgunluk.OlgunlukCihazi.Where(a => a.test_tarihi >= bas && a.test_tarihi <= bit && a.test_durum == true).ToList();

            //Inner Join Sorgusu
            var result = from oc in dbOlgunluk.OlgunlukCihazi
                         join os in dbOlgunluk.OlgunlukSensor on oc.ID equals os.testID
                         where oc.test_tarihi >= bas && oc.test_tarihi <= bit && oc.test_durum == true
                         select new OlgunlukViewModelDashboard
                         {
                             OlgunlukCihazList = new List<OlgunlukCihazi> { oc },
                             OlgunlukSensorList = new List<OlgunlukSensor> { os }
                         };


            var viewModel = new OlgunlukViewModelDashboard
            {
                OlgunlukCihazList = queryCihaz.ToList(),
                SensorCihazList = result.ToList()
            };

            TempData["ViewModel"] = viewModel; // ViewModel'i TempData'ye aktar
            return View(viewModel);
        }


        [HttpGet]
        public JsonResult GetSensorData(int id)
        {


            var query = dbOlgunluk.OlgunlukSensor
                .Where(a => a.testID == id)
                .Select(a => new OlgunlukTestVerileri
                {
                    Olgunluk = (double)a.olgunluk,
                    BasincDayanim = (double)a.basinc_dayanimi,
                    Sicaklik = (double)a.sensor_sicaklik,
                    EgilmeDayanim = (double)a.egilme_dayanimi,
                    SensorTarih = (DateTime)a.sensor_tarih,
                })
                .ToList();

            


            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}