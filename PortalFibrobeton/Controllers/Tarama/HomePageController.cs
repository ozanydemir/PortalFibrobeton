using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Models.Entity;
using System.Data.SqlClient;
using System.Globalization;
using PortalFibrobeton.Models.Class.Tarama;
using System.Data.Entity;

namespace PortalFibrobeton.Controllers
{

    
    public class HomePageController : Controller
    {

        TaramaEntities db = new TaramaEntities();
        // GET: HomePage
        //[Authorize]
        public ActionResult Index()
        {

            
            return View();
        }

        //[Authorize]
        public ActionResult Dashboard()
        {

            var sqlActivity = (from a in db.Tbl_Taramalar.SqlQuery("SELECT TOP 10 * FROM TBL_TARAMALAR where MalzemeT != 'BETON' order by IDT desc").ToList()
                              select new LastTenActivityModelItem
                              {
                                  Proje = a.ProjeT,
                                  Poz = a.PozT,
                                  Durum = a.DurumT,
                                  Tarih = a.TarihT,
                                  Sayi = a.SayıT,
                                  Konu = a.KonuT,
                                  Dokum = a.DökümT,
                                  Teslim = a.TeslimT

                              }).ToList();


            //Tüm Hafta
            var son7Gun = DateTime.Now.AddDays(-7);
            var sqlSorguDash = db.Tbl_Taramalar.Where(a => a.TarihT >= son7Gun).ToList();


            //Bugün Kayıtları
            var bugun = DateTime.Today;
            var sqlSorguBugun = db.Tbl_Taramalar.Where(a => a.TarihT == bugun).ToList();

            // Tüm hafta için taramaları al
            var baslangicTarihi = DateTime.Today.AddDays(-6); // Son 7 günü kapsayacak şekilde
            var bitisTarihi = DateTime.Today;

            // Veritabanından tarih ve sayıları al
            var taramalar = db.Tbl_Taramalar
                .Where(t => DbFunctions.TruncateTime(t.TarihT) >= baslangicTarihi && DbFunctions.TruncateTime(t.TarihT) <= bitisTarihi)
                .ToList() // Veritabanından verileri al
                .Select(t => new
                {
                    Tarih = t.TarihT,
                    Sayi = 1 // Her bir tarama için sayıyı 1 olarak kabul ediyoruz
                })
                .GroupBy(t => t.Tarih.DayOfWeek)
                .Select(g => new
                {
                    Gun = g.Key,
                    Sayi = g.Count()
                })
                .ToList();

            // Gün isimlerine göre sırala ve ViewBag'e ata
            var gunIsimleri = new Dictionary<DayOfWeek, string> {
                { DayOfWeek.Sunday, "Pazar" },
                { DayOfWeek.Monday, "Pazartesi" },
                { DayOfWeek.Tuesday, "Salı" },
                { DayOfWeek.Wednesday, "Çarşamba" },
                { DayOfWeek.Thursday, "Perşembe" },
                { DayOfWeek.Friday, "Cuma" },
                { DayOfWeek.Saturday, "Cumartesi" }
            };

            // Haftanın her günü için veri oluştur
            var taramaVerileri = Enumerable.Range(0, 7)
                .Select(i => baslangicTarihi.AddDays(i).DayOfWeek)
                .Select(gun => new {
                    GunAdi = gunIsimleri[gun],
                    Sayi = taramalar.FirstOrDefault(t => t.Gun == gun)?.Sayi ?? 0
                })
                .ToList();

            // Görünüme verileri aktar
            ViewBag.TaramaVerileri = taramaVerileri;


            //ViewModel
            var viewModel = new TaramaViewModel
            {
                LastTenActivity = sqlActivity,
                LastWeekResults = sqlSorguDash,
                TodayResults = sqlSorguBugun,

            };

            double YuzdeHesap(double a , double b)
            {
                double sonuc = ((a / b) * 100);
                sonuc = Math.Round(sonuc, 0);
                return sonuc;
            }

            var haftalikTotalTarama = sqlSorguDash.Select(a => a.PozBarkodT).Count();
            ViewBag.sonuc6TumHafta = YuzdeHesap(sqlSorguDash.Where(a => a.TeslimT == "HOL-6").Select(a => a.PozBarkodT).Count(), haftalikTotalTarama);
            ViewBag.sonuc9TumHafta = YuzdeHesap(sqlSorguDash.Where(a => a.TeslimT == "HOL-9").Select(a => a.PozBarkodT).Count(), haftalikTotalTarama);

            return View(viewModel);
        }

        public ActionResult ChangePassword()
        {
            int id = Convert.ToInt32(Session["UserID"]);

            var deger = db.Tbl_Users.FirstOrDefault(x => x.user_id == id);
            return View(deger);
        }

        [HttpPost]
        public ActionResult ChangePassword(string Password, string newPassword, string Confirmpwd)
        {
            int id = Convert.ToInt32(Session["UserID"]);

            var info = db.Tbl_Users.Where(a => a.user_id == id).FirstOrDefault();

            if (info.user_password == Password)
            {
                if (Confirmpwd == newPassword)
                {
                    info.Confirmpwd = Confirmpwd;
                    info.user_password = newPassword;
                    db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["successPassword"] = "<script>alert('Şifreniz başarıyla değiştirildi');</script>";
                }

                else
                {
                    TempData["errorPassword"] = "<script>alert('Yeni şifreniz birbiriyle eşleşmiyor. Lütfen tekrar deneyin!');</script>";
                }
            }

            else
            {
                TempData["errorPassword"] = "<script>alert('Eski şifreniz eksik veya hatalı. Lütfen şifrenizi kontrol edin!');</script>";
            }

            return RedirectToAction("Dashboard", "HomePage");
        }
        public ActionResult Page400()
        {
            Response.StatusCode = 400;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        public ActionResult Page403()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult Page500()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        public ActionResult Page503()
        {
            Response.StatusCode = 503;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}