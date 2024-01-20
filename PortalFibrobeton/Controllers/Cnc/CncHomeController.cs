using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Models.Entity;

namespace PortalFibrobeton.Controllers.Cnc
{
    public class CncHomeController : Controller
    {
        cncEntities dbCnc = new cncEntities();

        // GET: CncHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePageCnc()
        {

            var query = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri order by ID DESC").ToList()
                        select a;

            return View(query);
        }

        public ActionResult Deneme()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GecmisKayitEkleme()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GecmisKayitEkleme(CNC_1 p1, DateTime tarih, double ms, string makine)
        {
            try
            {
                
                p1.tarih = tarih.AddDays(1).AddHours(7).AddMinutes(59).AddSeconds(2);
                var filtreTarihBas = tarih.AddDays(1);
                var filtreTarihBit = filtreTarihBas.AddDays(1);

                //İlgili gündeki son kayıtları silme
                var eskiKayitlar = dbCnc.CNC_1.Where(a => a.tarih >= filtreTarihBas && a.tarih < filtreTarihBit && a.makine_adi == makine).ToList();
                foreach(var item in eskiKayitlar)
                {
                    dbCnc.CNC_1.Remove(item);
                };

                //Yeni veri kayıt ekleme
                p1.makine_adi = makine;
                p1.calisma_saati = ms;
                dbCnc.CNC_1.Add(p1);
                dbCnc.SaveChanges();

                TempData["successAdd"] = "Kayıt ekleme başarılı!";
            }
            catch(Exception)
            {
                TempData["errorAdd"] = "Kayıt ekleme sırasında hata oluştu!";
            }
            

            return View();
        }
    }
}