using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using PortalFibrobeton.Models.Entity;
using System.IO;
using System.Data.Entity.Validation;
using PortalFibrobeton.Attributelar;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using OneSignal.RestAPIv3.Client;

namespace PortalFibrobeton.Controllers
{
    public class AddScanController : Controller
    {
        // GET: AddScan

        
        TaramaEntities db = new TaramaEntities();

        PERA_FIBROEntities db1 = new PERA_FIBROEntities();

        PERA_FIBRO_ULTIMATEEntities db2 = new PERA_FIBRO_ULTIMATEEntities();

        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }


        
        [HttpGet]
        [Authorize]
        public ActionResult NewScan()
        {

            var idt = Convert.ToInt32(Session["IDUs"]);
            var roleSQL = from a in db1.KULLANICI.SqlQuery("SELECT * FROM KULLANICI").ToList()
                       select a;

            ViewBag.idt = idt;
            var role = roleSQL.Where(a => a.ID == idt).Select(a => a.YET_M_3D_TARAMA).FirstOrDefault();
            ViewBag.rolee = role;

            if (role == true)
            {
                return View();
            }
            else
            {
                //ViewBag.RoleError = "Bu işlemi yapmak için yetkiniz bulunmamaktadır!";
                return RedirectToAction("Dashboard", "HomePage");
            }

            //return View();
            
        }

        [Authorize]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "select")]
        public ActionResult NewScan(string poz)
        {
            poz = poz.ToUpper();
            try
            {
                //Poz
                string pozSQL = "SELECT * From KALIP_IS_EMRI_POZ where POZ_BARKOD =@p1";
                SqlParameter pozS1 = new SqlParameter("@p1", poz);
                object[] param = new object[] { pozS1 };
                var queryPOZ = db1.KALIP_IS_EMRI_POZ.SqlQuery(pozSQL, param).ToList();
                
                if(queryPOZ.Count > 0)
                {
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


                    //Malzeme

                    string malzemeSQL = "SELECT * From KALIP_IS_EMRI_KALIP_OLUSTUR where ISLEM_TURU = 'Kalıp' AND KALIP_IS_ID = @id";
                    SqlParameter malzemeP1 = new SqlParameter("@id", kalipIsId);
                    object[] param3 = new object[] { malzemeP1 };
                    var queryMalzeme = db1.KALIP_IS_EMRI_KALIP_OLUSTUR.SqlQuery(malzemeSQL, param3).ToList();
                    var malzeme = queryMalzeme.Select(a => a.ISLEM).FirstOrDefault();
                    ViewBag.malzeme = malzeme;


                    //Tarama Sayı

                    string sayıSQL = "SELECT * from Tbl_Taramalar where BarkodT = @p4";
                    SqlParameter sayıP1 = new SqlParameter("@p4", barkod);
                    object[] param4 = new object[] { sayıP1 };
                    var querySayı = db.Tbl_Taramalar.SqlQuery(sayıSQL, param4).ToList();

                    var sayı = querySayı.Select(a => a.SayıT).Count();
                    var sayıArtı = sayı + 1;
                    ViewBag.sayı = sayıArtı;

                    DateTime tarih = DateTime.Now;
                    var bugun = tarih.ToShortDateString();
                    ViewBag.bugun = bugun;


                    //Döküm Türü

                    string dokumSQL = "SELECT * FROM SIPARIS_POZ where POZ_NO = @p5";
                    SqlParameter dokumP1 = new SqlParameter("@p5", pozNO);
                    object[] param5 = new object[] { dokumP1 };
                    var querydokum = db1.SIPARIS_POZ.SqlQuery(dokumSQL, param5).ToList();
                    var dokum = querydokum.Select(a => a.POZ_TURU).LastOrDefault();
                    ViewBag.dokum = dokum;

                    string hol9 = "HOL-9";
                    string hol6 = "HOL-6";
                    //string bos = null;

                    if (dokum == "UHPC")
                    {
                        var dokumTeslim = hol9;
                        ViewBag.dokumTeslim = dokumTeslim;
                    }
                    if (dokum == "GRC")
                    {
                        var dokumTeslim = hol6;
                        ViewBag.dokumTeslim = dokumTeslim;
                    }
                }

                else
                {
                    
                }

                

            }

            catch (SqlException)
            {
                TempData["errorQuery"] = "Poz barkod numarası eksik veya hatalı!";
            }



            return View();
        }


        [Authorize]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "save")]
        public async Task<ActionResult> Save(Tbl_Taramalar p1, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4, HttpPostedFileBase image5, HttpPostedFileBase image6
            , HttpPostedFileBase image7, HttpPostedFileBase image8, HttpPostedFileBase image9, HttpPostedFileBase image10)
        {

            try
            {
                if (image1 != null)
                {
                    p1.ResimT1 = new byte[image1.ContentLength];
                    image1.InputStream.Read(p1.ResimT1, 0, image1.ContentLength);

                }

                if (image2 != null)
                {
                    p1.ResimT2 = new byte[image2.ContentLength];
                    image2.InputStream.Read(p1.ResimT2, 0, image2.ContentLength);
                }

                if (image3 != null)
                {
                    p1.ResimT3 = new byte[image3.ContentLength];
                    image3.InputStream.Read(p1.ResimT3, 0, image3.ContentLength);
                }

                if (image4 != null)
                {
                    p1.ResimT4 = new byte[image4.ContentLength];
                    image4.InputStream.Read(p1.ResimT4, 0, image4.ContentLength);
                }

                if (image5 != null)
                {
                    p1.ResimT5 = new byte[image5.ContentLength];
                    image5.InputStream.Read(p1.ResimT5, 0, image5.ContentLength);
                }

                if (image6 != null)
                {
                    p1.ResimT6 = new byte[image6.ContentLength];
                    image6.InputStream.Read(p1.ResimT6, 0, image6.ContentLength);
                }

                if (image7 != null)
                {
                    p1.ResimT7 = new byte[image7.ContentLength];
                    image7.InputStream.Read(p1.ResimT7, 0, image7.ContentLength);
                }

                if (image8 != null)
                {
                    p1.ResimT8 = new byte[image8.ContentLength];
                    image8.InputStream.Read(p1.ResimT8, 0, image8.ContentLength);
                }

                if (image9 != null)
                {
                    p1.ResimT9 = new byte[image9.ContentLength];
                    image9.InputStream.Read(p1.ResimT9, 0, image9.ContentLength);
                }

                if (image10 != null)
                {
                    p1.ResimT10 = new byte[image10.ContentLength];
                    image10.InputStream.Read(p1.ResimT10, 0, image10.ContentLength);
                }

                string reporter = Session["UserName"].ToString();

                if (reporter != null)
                {
                    p1.RaporT = reporter;
                }
                else
                {
                    p1.RaporT = null;
                }

                db.Tbl_Taramalar.Add(p1);
                db.SaveChanges();


                //var bildirimQuery = db.Tbl_Taramalar.ToList();
                //var bildirimPoz = bildirimQuery.Select(a => a.PozT).Last();
                //var bildirimDurum = bildirimQuery.Select(a => a.DurumT).Last();


                //var client = new OneSignalClient("MjkwMTc0YTEtZWQwNi00MmY0LTgzMjMtNzk4MDdlYTgxMzQ4");

                //var options = new NotificationCreateOptions
                //{
                //    AppId = new Guid("a69d4d12-b09b-4f12-9001-9a7d58d8eaa4"),
                //    IncludedSegments = new List<string> { "All" },
                //    Contents = new Dictionary<string, string> { { "en", bildirimPoz + " poz numaralı kalıp tarandı ve " + bildirimDurum + " verildi!"   } }
                //};



                //NotificationCreateResult result = await client.Notifications.CreateAsync(options);


                TempData["successAdd"] = "Tarama kaydı başarılı!";

            }


            catch (Exception)
            {
                TempData["errorAdd"] = "Bilgiler eksik veya hatalı!";
            }



            return View();

        }



        [HttpGet]
        [Authorize]
        public ActionResult TaramaEkleNP()
        {

            var idt = Convert.ToInt32(Session["IDUs"]);
            var roleSQL = from a in db1.KULLANICI.SqlQuery("SELECT * FROM KULLANICI").ToList()
                          select a;

            ViewBag.idt = idt;
            var role = roleSQL.Where(a => a.ID == idt).Select(a => a.YET_M_3D_TARAMA).FirstOrDefault();
            ViewBag.rolee = role;

            if (role == true)
            {
                return View();
            }
            else
            {
                //ViewBag.RoleError = "Bu işlemi yapmak için yetkiniz bulunmamaktadır!";
                return RedirectToAction("Dashboard", "HomePage");
            }

            //return View();

        }

        [HttpPost]
        public ActionResult TaramaEkleNP(string isEmri)
        {
            var kalipTasarimID = db2.KALIP_IS_EMRI.Where(a => a.IS_EMRI_BARKOD == isEmri).Select(a => a.KALIP_TASARIM_ID).LastOrDefault();
            var kalipTasarımPozList = db2.KALIP_TASARIM_POZ.Where(a => a.KALIP_TASARIM_ID == kalipTasarimID).ToList();
            List<POZ_KART_NP> pozAdlari = new List<POZ_KART_NP>();
            foreach(var i in kalipTasarımPozList.Select(a => a.POZ_ID))
            {
                var pozList = db2.POZ_KART.Where(a => a.ID == i).ToList();
                pozAdlari.AddRange(pozList);

            }
            ViewBag.pozList = pozAdlari;

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult NewScanConc()
        {
            var idt = Convert.ToInt32(Session["IDUs"]);
            var roleSQL = from a in db1.KULLANICI.SqlQuery("SELECT * FROM KULLANICI").ToList()
                          select a;

            ViewBag.idt = idt;
            var role = roleSQL.Where(a => a.ID == idt).Select(a => a.YET_M_3D_TARAMA).FirstOrDefault();
            ViewBag.rolee = role;

            if (role == true)
            {
                return View();
            }
            else
            {
                //ViewBag.RoleError = "Bu işlemi yapmak için yetkiniz bulunmamaktadır!";
                return RedirectToAction("Dashboard", "HomePage");
            }
        }



        
        [HttpPost]
        [MultipleButtonConc(Ad = "aksiyon", Arguman = "sec")]
        public ActionResult NewScanConc(string BarkodT)
        {

            try
            {
                //Barkod Poz
                string pozSQL1 = "SELECT * FROM KALIP_IS_EMRI_DOKUM Where DOKUM_BARKOD = @p1";
                SqlParameter pozP12 = new SqlParameter("@p1", BarkodT);
                object[] param11 = new object[] { pozP12 };
                var queryPoz1 = db1.KALIP_IS_EMRI_DOKUM.SqlQuery(pozSQL1, param11).ToList();

                var panelBarkod = queryPoz1.Select(a => a.DOKUM_BARKOD).LastOrDefault();
                ViewBag.panelBarkod = panelBarkod;
                var poz = queryPoz1.Select(a => a.POZ_NO).LastOrDefault();
                ViewBag.pozPanel = poz;
                var pozIDPanel = queryPoz1.Select(a1 => a1.POZ_ID).LastOrDefault();
                ViewBag.pozIDPanel = pozIDPanel;


                //Proje
                string projeSQL = "Select * from KALIP_IS_EMRI where POZ_ID = @b2";
                SqlParameter projeP12 = new SqlParameter("@b2", pozIDPanel);
                object[] param12 = new object[] { projeP12 };
                var queryProje = db1.KALIP_IS_EMRI.SqlQuery(projeSQL, param12).ToList();

                var proje = queryProje.Select(a => a.ANA_POZ_PROJE_ADI).LastOrDefault();
                ViewBag.projePanel = proje;
                var panelM2 = queryProje.Select(a => a.KALIP_M2).LastOrDefault();
                ViewBag.panelM2 = panelM2;


                //Tarama Sayı

                string sayı = "Select * from Tbl_Taramalar where BarkodT = @barkod";
                SqlParameter sayıP12 = new SqlParameter("@barkod", BarkodT);
                object[] param13 = new object[] { sayıP12 };
                var querySayı = db.Tbl_Taramalar.SqlQuery(sayı, param13).ToList();
                var sayıPanel = querySayı.Select(a => a.SayıT).Count();
                var sayıPanelArtı = sayıPanel + 1;
                ViewBag.sayıPanel = sayıPanelArtı;

                DateTime bugun = DateTime.Now;
                var today = bugun.ToShortDateString();
                ViewBag.today = today;


                //Döküm Türü

                string dokumSQL = "SELECT * FROM SIPARIS_POZ where POZ_NO = @p5";
                SqlParameter dokumP1 = new SqlParameter("@p5", poz);
                object[] param5 = new object[] { dokumP1 };
                var querydokum = db1.SIPARIS_POZ.SqlQuery(dokumSQL, param5).ToList();
                var dokum = querydokum.Select(a => a.POZ_TURU).LastOrDefault();
                ViewBag.dokumC = dokum;


            }

            catch (Exception)
            {
                TempData["errorQueryC"] = "Barkod numarası eksik veya hatalı!";
            }


            return View();
        }


        [Authorize]
        [HttpPost]
        [MultipleButtonConc(Ad = "aksiyon", Arguman = "kaydet")]
        public ActionResult SaveConc(Tbl_Taramalar p2, HttpPostedFileBase imageP1, HttpPostedFileBase imageP2, HttpPostedFileBase imageP3, HttpPostedFileBase imageP4, HttpPostedFileBase imageP5, HttpPostedFileBase imageP6
            , HttpPostedFileBase imageP7, HttpPostedFileBase imageP8, HttpPostedFileBase imageP9, HttpPostedFileBase imageP10)
        {
            try
            {
                if (imageP1 != null)
                {
                    p2.ResimT1 = new byte[imageP1.ContentLength];
                    imageP1.InputStream.Read(p2.ResimT1, 0, imageP1.ContentLength);

                }

                if (imageP2 != null)
                {
                    p2.ResimT2 = new byte[imageP2.ContentLength];
                    imageP2.InputStream.Read(p2.ResimT2, 0, imageP2.ContentLength);
                }

                if (imageP3 != null)
                {
                    p2.ResimT3 = new byte[imageP3.ContentLength];
                    imageP3.InputStream.Read(p2.ResimT3, 0, imageP3.ContentLength);
                }

                if (imageP4 != null)
                {
                    p2.ResimT4 = new byte[imageP4.ContentLength];
                    imageP4.InputStream.Read(p2.ResimT4, 0, imageP4.ContentLength);
                }

                if (imageP5 != null)
                {
                    p2.ResimT5 = new byte[imageP5.ContentLength];
                    imageP5.InputStream.Read(p2.ResimT5, 0, imageP5.ContentLength);
                }

                if (imageP6 != null)
                {
                    p2.ResimT6 = new byte[imageP6.ContentLength];
                    imageP6.InputStream.Read(p2.ResimT6, 0, imageP6.ContentLength);
                }

                if (imageP7 != null)
                {
                    p2.ResimT7 = new byte[imageP7.ContentLength];
                    imageP7.InputStream.Read(p2.ResimT7, 0, imageP7.ContentLength);
                }

                if (imageP8 != null)
                {
                    p2.ResimT8 = new byte[imageP8.ContentLength];
                    imageP8.InputStream.Read(p2.ResimT8, 0, imageP8.ContentLength);
                }

                if (imageP9 != null)
                {
                    p2.ResimT9 = new byte[imageP9.ContentLength];
                    imageP9.InputStream.Read(p2.ResimT9, 0, imageP9.ContentLength);
                }

                if (imageP10 != null)
                {
                    p2.ResimT10 = new byte[imageP10.ContentLength];
                    imageP10.InputStream.Read(p2.ResimT10, 0, imageP10.ContentLength);
                }

                p2.DurumT = "BOŞ";
                db.Tbl_Taramalar.Add(p2);
                db.SaveChanges();
                TempData["successAddC"] = "Tarama kaydı başarılı!";
            }

            catch (Exception)
            {
                TempData["errorAddC"] = "Bilgiler eksik veya hatalı!";
            }

            return View();
        }
    }
}