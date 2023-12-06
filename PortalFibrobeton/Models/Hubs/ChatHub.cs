using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PortalFibrobeton.Models.Entity;


namespace PortalFibrobeton
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void Deneme(string sicak)
        {
            Clients.All.deneme(sicak);
        }
        //Ortam Sıcaklık
        public void SicaklikOlg1(string sicaklik)
        {
            Clients.All.updateTopicSicaklik1(sicaklik);
        }

        //Timer verisini gönderme
        public void SendCurrentTimeElapsed(string topic)
        {
            var mqttService = MvcApplication.MqttServiceInstance;
            var timeElapsed = mqttService.GetCurrentTimeElapsed(topic);
            Clients.Caller.updateTimeElapsed(topic, timeElapsed.ToString(@"hh\:mm\:ss"));
        }
        //Chart için ortalama sıcaklık verisi 10 dakikada bir
        public void OrtalamaSicaklik(string topic,string sicaklik)
        {
            Clients.All.averageTemp(topic, sicaklik);
        }
        //Gerekli Değil gibi duruyor
        public void TahminiKalan(string kalanSure, string topic)
        {
            Clients.All.TahminiKalan(kalanSure, topic);
        }
        //Olgunluğa Kalan Yüzde Chart
        public void TotalOlgunluk(string totalOlg, string topic, string tahminiGevsetmeZaman)
        {
            Clients.All.totalOlg(topic, totalOlg, tahminiGevsetmeZaman);
        }
        //Olgunluk Toplam Veri
        public void Olgunluk(string topic, double olgunluk)
        {
            Clients.All.olg(topic,olgunluk);
        }
        //Sıcaklık verileri chart
        public void SicaklikSensor(string topic, string sicaklik)
        {
            Clients.All.sicaklikSensor(topic, sicaklik);
        }
        
        //Min. Sıcaklık
        public void MinSıcaklık(string topic, string minSicaklik)
        {
            Clients.All.minTemp(topic, minSicaklik);
        }

        //Timeout Image
        public void Timeout(string topic)
        {
            Clients.All.Timeout(topic);
        }

        //Gevşetme Image
        public void Gevset(string topic)
        {
            Clients.All.Gevset(topic);
        }

        public void PublishData(string topic, string payload)
        {
            MvcApplication.MqttServiceInstance.PublishData(topic, payload);
        }

    }
}