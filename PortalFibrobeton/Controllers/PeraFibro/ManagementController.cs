using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class ManagementController : Controller
    {
        // GET: Management

        DateTime bugun = DateTime.Now.Date;
        
        KatsayiHesabiEntities db = new KatsayiHesabiEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AnketOlustur()
        {
            var today = bugun.ToShortDateString();
            ViewBag.today= today;

            var query = from a in db.Anketler.SqlQuery("SELECT * FROM Anketler").ToList()
                        select a;
            var sablonList = query.Select(a => a.AnketAdi).ToList();
            return View(query);

        }

        [HttpPost]
        public ActionResult AnketOlustur(Anketler p1, string anketname, string checkTemp,int? discountselection)
        {

            var checkAnketAd = (from a in db.Anketler
                                where a.AnketAdi.ToUpper() == anketname.ToUpper()
                                select new { anketname }).FirstOrDefault();
            

            if (checkAnketAd == null)
            {
                if(checkTemp == "Yes")
                {
                    var sablonquery = from a in db.Anketler.Where(a => a.AnketID == discountselection).ToList()
                                      select a;

                    Session["discountselection"] = discountselection;
                    Session["checkTemp"] = checkTemp;
                    p1.AnketAdi = anketname;
                    p1.AnketiOlusturan = Session["UserName"].ToString();
                    //p1.ProjeAdi = projename;
                    p1.AnketiOlusturmaTarihi = bugun;
                    db.Anketler.Add(p1);
                    db.SaveChanges();

                    return RedirectToAction("ProjeKayit", "Management");
                }
                else
                {
                    Session["discountselection"] = null;
                    //Şablon eklenirken burası aktif edilecek
                    //Session["checkTemp"] = checkTemp;
                    Session["checkTemp"] = "No";
                    p1.AnketAdi = anketname;
                    p1.AnketiOlusturan = Session["UserName"].ToString();
                    //p1.ProjeAdi = projename;
                    p1.AnketiOlusturmaTarihi = bugun;
                    db.Anketler.Add(p1);
                    db.SaveChanges();

                    return RedirectToAction("ProjeKayit", "Management");
                }
                              
            }

            else
            {
                TempData["AnketAdiError"] = "Bu anket adı kullanılıyor. Lütfen farklı bir anket ismi verin!";
                return RedirectToAction("AnketOlustur", "Management");
            }
           
        }

        public ActionResult ProjeKayit()
        {
            if (Session["discountselection"] == null)
            {
                return View();
            }
            else
            {
                var modelQuery = from a in db.AnketProjeler.SqlQuery("SELECT * FROM AnketProjeler").ToList()
                                 where a.AnketID == Convert.ToInt16(Session["discountselection"])
                                 select a;

                ViewBag.projeNo = modelQuery.Select(a => a.ProjeAdi).Count();
                
                return View(modelQuery);
            }
        }
        public JsonResult InsertProjeler(List<AnketProjeler> projeler)
        {
            var anketQuery = db.Anketler.SqlQuery("SELECT * FROM Anketler").ToList();
            var anketID = anketQuery.Select(a => a.AnketID).Last();
            foreach (AnketProjeler s in projeler)
            {
                s.AnketID = anketID;
                db.AnketProjeler.Add(s);
            }
            int kayitlarikaydet = db.SaveChanges();
            return Json(kayitlarikaydet);

        }


        public ActionResult Sorular()
        {

            var anketQuery = db.Anketler.SqlQuery("SELECT * FROM Anketler").ToList();
            var anketID = anketQuery.Select(a => a.AnketID).Last();
            var anketName = anketQuery.Select(a => a.AnketAdi).Last();
            ViewBag.anketID = anketID;
            ViewBag.anketAdi = anketName;

            var soruQuery = db.AnketSorular.SqlQuery("SELECT * FROM AnketSorular").ToList();

            var soruNo = soruQuery.Where(a => a.AnketID == anketID && a.AsamaAdi == "Proje Aşaması").Select(a => a.SoruNo).LastOrDefault();
            if (soruNo == null)
            {
                soruNo = 1;
                ViewBag.soruNo = soruNo;
            }
            else
            {
                soruNo = soruNo + 1;
                ViewBag.soruNo = soruNo;
            }

            if(Session["discountselection"] == null)
            {
                return View(db.AnketSorular);
            }
            else
            {
                var modelQuery = from a in db.AnketSorular.SqlQuery("SELECT * FROM AnketSorular").ToList()
                                 where a.AnketID == Convert.ToInt32(Session["discountselection"])
                                 select a;
                return View(modelQuery);
            }
           
            
        }  
        
        public JsonResult InsertQuestions(List<AnketSorular> sorular)
        {
            var anketQuery = db.Anketler.SqlQuery("SELECT * FROM Anketler").ToList();
            var anketID = anketQuery.Select(a => a.AnketID).Last();
            foreach (AnketSorular s in sorular)
            {
                s.AnketID = anketID;
                db.AnketSorular.Add(s);
            }

            int kayıtlarıKaydet = db.SaveChanges();
            return Json(kayıtlarıKaydet);
        }
       

        [HttpGet]
        public ActionResult AnketSec()
        {
            var query = db.Anketler.SqlQuery("SELECT * FROM Anketler").ToList();
            
            return View(query);
        }

        public JsonResult GetProjeler(int id)
        {
            string kullaniciAdi = Session["UserName"].ToString();

            var cozulenProjeID = db.AnketCevaplar
                .Where(a => a.AnketID == id && a.AnketiCozen == kullaniciAdi)
                .Select(a => a.ProjeID)
                .ToList();


            var projeler = db.AnketProjeler
                .Where(b => b.AnketID == id && !cozulenProjeID.Contains(b.ID))
                .Select(b => new { b.ID, b.ProjeAdi })
                .ToList();


            return Json(projeler, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public ActionResult AnketSec(int anketid, int selectedProjectID)
        {
            var query = from a in db.AnketSorular.SqlQuery("SELECT * FROM AnketSorular").ToList()
                        where a.AnketID == anketid
                        select a;

            Session["anketid"] = anketid;
            Session["projeid"] = selectedProjectID;
            string kullaniciAd = Session["UserName"].ToString();

            return RedirectToAction("Test", "Management");


        }

        
        public ActionResult Test()
        {

            var query = from a in db.AnketSorular.SqlQuery("SELECT * FROM AnketSorular").ToList()
                        where a.AnketID == Convert.ToInt16(Session["anketid"])                
                        select a;

            var anketAdi = from a in db.Anketler.SqlQuery("SELECT * FROM Anketler").ToList()
                           where a.AnketID == Convert.ToInt16(Session["anketid"])
                           select a;


            ViewBag.anketAdi = anketAdi.Select(a => a.AnketAdi).FirstOrDefault();
            ViewBag.tarih = bugun;
            ViewBag.cozen = Session["UserName"].ToString();
            ViewBag.projeID = Session["projeid"].ToString();

            return View(query);
        }

        public JsonResult InsertAnswers(List<AnketCevaplar> cevaplar)
        {
            //var today = bugun.ToShortDateString();
            int anketID = 0;
            var projeID = 0;
            foreach (AnketCevaplar c in cevaplar)
            {
                db.AnketCevaplar.Add(c);
                anketID = (int)c.AnketID;
                projeID = (int)c.ProjeID;
                

            }

            var sorularQuery = from a in db.AnketSorular.SqlQuery("SELECT * FROM AnketSorular")
                               where a.AnketID == anketID
                               select a;

            var projeGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Proje Aşaması").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var kalipGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Kalıp Aşaması").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var karkasGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Karkas").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var malzemeGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Malzeme").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var dokumGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Döküm").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var sokumGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Söküm").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var yuzeyGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Yüzey").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var kaliteGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Kalite").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var sevkiyatGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Sevkiyat").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var maddiGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Maddi Faktör").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();
            var montajGenelKatsayi = sorularQuery.Where(a => a.AsamaAdi == "Montaj").Select(a => a.AsamaKatsayiDegeri).FirstOrDefault();

            //Proje Aşaması Kayıt
            var projeAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Kalıp Aşaması" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));


            if (!cevaplar.Any(a => a.AsamaAdi =="Proje Aşaması") && projeAsamasiCarpim == 1)
            {
                projeAsamasiCarpim = 0;
            }

            if(projeAsamasiCarpim != 0)
            {
                AnketSonuclar projeAsamasiSonuc = new AnketSonuclar();
                projeAsamasiSonuc.Sonuc = (projeAsamasiCarpim * projeGenelKatsayi) / 100;
                projeAsamasiSonuc.AsamaAdi = "Proje Aşaması";
                projeAsamasiSonuc.AnketID = anketID;
                projeAsamasiSonuc.ProjeID = projeID;
                projeAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                projeAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(projeAsamasiSonuc);
            }


            //Kalıp Aşaması Kayıt
            var kalipAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Kalıp Aşaması" && a.CevapNo != 0)
                 .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi =="Kalıp Aşaması") && kalipAsamasiCarpim == 1)
            {
                kalipAsamasiCarpim = 0;
            }

            if (kalipAsamasiCarpim != 0)
            {
                AnketSonuclar kalipAsamasiSonuc = new AnketSonuclar();
                kalipAsamasiSonuc.Sonuc = (Convert.ToDouble(kalipAsamasiCarpim) * kalipGenelKatsayi) / 100;
                kalipAsamasiSonuc.AsamaAdi = "Kalıp Aşaması";
                kalipAsamasiSonuc.AnketID = anketID;
                kalipAsamasiSonuc.ProjeID = projeID;
                kalipAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                kalipAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(kalipAsamasiSonuc);
            }



            //Karkas Aşaması Kayıt
            var karkasAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Karkas" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Karkas") && karkasAsamasiCarpim == 1)
            {
                karkasAsamasiCarpim = 0;
            }

            if (karkasAsamasiCarpim != 0)
            {
                AnketSonuclar karkasAsamasiSonuc = new AnketSonuclar();
                karkasAsamasiSonuc.Sonuc = (karkasAsamasiCarpim * karkasGenelKatsayi) / 100;
                karkasAsamasiSonuc.AsamaAdi = "Karkas";
                karkasAsamasiSonuc.AnketID = anketID;
                karkasAsamasiSonuc.ProjeID = projeID;
                karkasAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                karkasAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(karkasAsamasiSonuc);
            }


            //Malzeme Aşaması Kayıt
            var malzemeAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Malzeme" && a.CevapNo != 0)
                .Aggregate(1.0,(acc,a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Malzeme") && malzemeAsamasiCarpim == 1)
            {
                malzemeAsamasiCarpim = 0;
            }

            if (malzemeAsamasiCarpim != 0)
            {
                AnketSonuclar MalzemeAsamasiSonuc = new AnketSonuclar();
                MalzemeAsamasiSonuc.Sonuc = (malzemeAsamasiCarpim * malzemeGenelKatsayi) / 100;
                MalzemeAsamasiSonuc.AsamaAdi = "Malzeme";
                MalzemeAsamasiSonuc.AnketID = anketID;
                MalzemeAsamasiSonuc.ProjeID = projeID;
                MalzemeAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                MalzemeAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(MalzemeAsamasiSonuc);
            }


            //Döküm Aşaması Kayıt
            var dokumAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Döküm" && a.CevapNo != 0)
                .Aggregate(1.0,(acc,a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Döküm") && dokumAsamasiCarpim == 1)
            {
                dokumAsamasiCarpim = 0;
            }

            if (dokumAsamasiCarpim != 0)
            {
                AnketSonuclar dokumAsamasiSonuc = new AnketSonuclar();
                dokumAsamasiSonuc.Sonuc = (dokumAsamasiCarpim * dokumGenelKatsayi) / 100;
                dokumAsamasiSonuc.AsamaAdi = "Döküm";
                dokumAsamasiSonuc.AnketID = anketID;
                dokumAsamasiSonuc.ProjeID = projeID;
                dokumAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                dokumAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(dokumAsamasiSonuc);
            }



            //Söküm Aşaması Kayıt
            var sokumAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Söküm" && a.CevapNo != 0)
                .Aggregate(1.0,(acc,a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Söküm") && sokumAsamasiCarpim == 1)
            {
                sokumAsamasiCarpim = 0;
            }

            if (sokumAsamasiCarpim != 0)
            {
                AnketSonuclar sokumAsamasiSonuc = new AnketSonuclar();
                sokumAsamasiSonuc.Sonuc = (sokumAsamasiCarpim * sokumGenelKatsayi) / 100;
                sokumAsamasiSonuc.AsamaAdi = "Söküm";
                sokumAsamasiSonuc.AnketID = anketID;
                sokumAsamasiSonuc.ProjeID = projeID;
                sokumAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                sokumAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(sokumAsamasiSonuc);
            }



            //Yüzey Aşaması Kayıt
            var yuzeyAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Yüzey" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Yüzey") && yuzeyAsamasiCarpim == 1)
            {
                yuzeyAsamasiCarpim = 0;
            }

            if (yuzeyAsamasiCarpim != 0)
            {
                AnketSonuclar yuzeyAsamasiSonuc = new AnketSonuclar();
                yuzeyAsamasiSonuc.Sonuc = (yuzeyAsamasiCarpim * yuzeyGenelKatsayi) / 100;
                yuzeyAsamasiSonuc.AsamaAdi = "Yüzey";
                yuzeyAsamasiSonuc.AnketID = anketID;
                yuzeyAsamasiSonuc.ProjeID = projeID;
                yuzeyAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                yuzeyAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(yuzeyAsamasiSonuc);
            }


            //Kalite Aşaması Kayıt
            var kaliteAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Kalite" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Kalite") && kaliteAsamasiCarpim == 1)
            {
                kaliteAsamasiCarpim = 0;
            }

            if (kaliteAsamasiCarpim != 0)
            {
                AnketSonuclar kaliteAsamasiSonuc = new AnketSonuclar();
                kaliteAsamasiSonuc.Sonuc = (kaliteAsamasiCarpim * kaliteGenelKatsayi) / 100;
                kaliteAsamasiSonuc.AsamaAdi = "Kalite";
                kaliteAsamasiSonuc.AnketID = anketID;
                kaliteAsamasiSonuc.ProjeID = projeID;
                kaliteAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                kaliteAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(kaliteAsamasiSonuc);
            }


            //Sevkiyat Aşaması Kayıt
            var sevkiyatAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Sevkiyat" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Sevkiyat") && sevkiyatAsamasiCarpim == 1)
            {
                sevkiyatAsamasiCarpim = 0;
            }

            if (sevkiyatAsamasiCarpim != 0)
            {
                AnketSonuclar sevkiyatAsamasiSonuc = new AnketSonuclar();
                sevkiyatAsamasiSonuc.Sonuc = (sevkiyatAsamasiCarpim * sevkiyatGenelKatsayi) / 100;
                sevkiyatAsamasiSonuc.AsamaAdi = "Sevkiyat";
                sevkiyatAsamasiSonuc.AnketID = anketID;
                sevkiyatAsamasiSonuc.ProjeID = projeID;
                sevkiyatAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                sevkiyatAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(sevkiyatAsamasiSonuc);
            }


            //Maddi Aşaması Kayıt
            var maddiAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Maddi Faktör" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));

            if (!cevaplar.Any(a => a.AsamaAdi == "Maddi Faktör") && maddiAsamasiCarpim == 1)
            {
                maddiAsamasiCarpim = 0;
            }

            if (maddiAsamasiCarpim != 0)
            {
                AnketSonuclar maddiAsamasiSonuc = new AnketSonuclar();
                maddiAsamasiSonuc.Sonuc = (maddiAsamasiCarpim * maddiGenelKatsayi) / 100;
                maddiAsamasiSonuc.AsamaAdi = "Maddi Faktör";
                maddiAsamasiSonuc.AnketID = anketID;
                maddiAsamasiSonuc.ProjeID = projeID;
                maddiAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                maddiAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(maddiAsamasiSonuc);
            }


            //Montaj Aşaması Kayıt
            var montajAsamasiCarpim = cevaplar
                .Where(a => a.AsamaAdi == "Montaj" && a.CevapNo != 0)
                .Aggregate(1.0, (acc, a) => ((double)(acc * a.CevapNo)));


            if (!cevaplar.Any(a => a.AsamaAdi == "Montaj") && montajAsamasiCarpim == 1)
            {
                montajAsamasiCarpim = 0;
            }

            if (montajAsamasiCarpim != 0)
            {
                AnketSonuclar montajAsamasiSonuc = new AnketSonuclar();
                montajAsamasiSonuc.Sonuc = (montajAsamasiCarpim * montajGenelKatsayi) / 100;
                montajAsamasiSonuc.AsamaAdi = "Montaj";
                montajAsamasiSonuc.AnketID = anketID;
                montajAsamasiSonuc.ProjeID = projeID;
                montajAsamasiSonuc.AnketiCozen = Session["UserName"].ToString();
                montajAsamasiSonuc.CozumTarihi = bugun;
                db.AnketSonuclar.Add(montajAsamasiSonuc);
            }

            //db.SaveChanges();
            int cevaplarıkaydet = db.SaveChanges();
            return Json(cevaplarıkaydet);
            
        }

        [HttpGet]
        public ActionResult AnketSonuclari()
        {

            var model = new AnketSonucModel
            {
                AnketList = db.Anketler.ToList(),
                SelectedAnketID = 0
            };
            return View(model);
        }

        public JsonResult ProjeListeleCozum(int id)
        {
            var projeler = db.AnketProjeler
                .Where(a => a.AnketID == id)
                .Select(b => new { b.ID, b.ProjeAdi })
                .ToList();

            return Json(projeler, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AnketSonuclari(AnketSonucModel model, int anketid = 0, int projeid = 0)
        {


            var anketSonuclari = (from a in db.Anketler
                                  join s in db.AnketSonuclar on a.AnketID equals s.AnketID
                                  where a.AnketID == anketid && s.ProjeID == projeid 
                                  group s by s.AnketiCozen into sonucGruplu
                                  select sonucGruplu).ToList();

            var toplamSonuc = db.AnketSonuclar.Where(a => a.AnketID == anketid && a.ProjeID == projeid).Select(a => a.Sonuc).Sum();
            var toplamCozenSayi = db.AnketSonuclar.Where(a => a.AnketID == anketid && a.ProjeID == projeid).Select(a => a.AnketiCozen).Distinct().Count();
            ViewBag.toplamSonuc = toplamSonuc / toplamCozenSayi;

            var cozenKisiSayi = from a in db.AnketSonuclar.Where(a => a.AnketID == anketid && a.ProjeID == projeid).ToList()
                                select a;


            var checkCozenKisi = cozenKisiSayi.Select(a => a.AnketiCozen).Distinct().Count();
            ViewBag.cozenKisiSayi = checkCozenKisi;

            if( anketid == 0 || projeid == 0)
            {
                TempData["errorParametre"] = "Lütfen geçerli bir anket ve proje seçin!";
                model.AnketList = db.Anketler.ToList();
                return View(model);
            }

            if (ModelState.IsValid && checkCozenKisi >= 3)
            {

                model.AnketList = db.Anketler.ToList();
                model.AnketSonuclari = anketSonuclari;

                return View(model);

            }
            else if(checkCozenKisi < 3)
            {
                TempData["errorCozenKisiSayi"] = "En az 3 kişi bu anketi çözmeden sonucu görüntüleyemezsiniz!";

                var modelNull = new AnketSonucModel
                {
                    AnketList = db.Anketler.ToList(),
                    SelectedAnketID = 0
                };
                return View(modelNull);
            }
            else
            {
                return View();
            }
        }

        public ActionResult YeniProjeEkleAnket()
        {
            return View();
        }

        [HttpGet]
        public ActionResult YeniProjeEkle()
        {
            var model = new AnketSonucModel
            {
                AnketList = db.Anketler.ToList(),
                SelectedAnketID = 0
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult YeniProjeEkle(AnketSonucModel model, int anketid = 0)
        {
            var projeAdlari = from a in db.AnketProjeler
                              .Where(a => a.AnketID == anketid).ToList()
                              select a;

            Session["projeEkleAnketID"] = anketid;

            model.AnketList = db.Anketler.ToList();
            model.ProjeList = projeAdlari.ToList();
            return View(model);
        }


        [HttpPost]
        public ActionResult UpdateProjeler(List<AnketProjeler> projeler)
        {
            try
            {
                int anketIDProjeKaydet = Convert.ToInt16(Session["projeEkleAnketID"]);
                var eskiProjeler = db.AnketProjeler.Where(a => a.AnketID == anketIDProjeKaydet).ToList();
                var yeniProjeler = new List<AnketProjeler>();

                foreach (var proje in projeler)
                {
                    if(!eskiProjeler.Any(p => p.ProjeAdi == proje.ProjeAdi))
                    {
                        proje.AnketID = anketIDProjeKaydet;
                        yeniProjeler.Add(proje);    
                    }
                }

                db.AnketProjeler.AddRange(yeniProjeler);
                int kaydet = db.SaveChanges();
                return Json(kaydet);

                //return Json(new { success = true, message = "Projeler başarıyla kaydedildi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
