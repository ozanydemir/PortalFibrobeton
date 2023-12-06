using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using PortalFibrobeton.Models.Entity;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using OneSignal.RestAPIv3.Client;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using PortalFibrobeton.Attributelar;
using System.Diagnostics;

namespace PortalFibrobeton.Controllers.Tarama
{
    public class TaramaEkleNPController : Controller
    {
        TaramaEntities db = new TaramaEntities();
        PERA_FIBROEntities db1 = new PERA_FIBROEntities();
        PERA_FIBRO_ULTIMATEEntities db2 = new PERA_FIBRO_ULTIMATEEntities();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddScanNP()
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
        public ActionResult AddScanNP(string pozBarkod)
        {
            pozBarkod = pozBarkod.ToUpper();
            try
            {
                //DIPB
                var dipbQuery = db2.DOKUM_IS_EMRI_POZ.Where(a => a.DOKUM_POZ_BARKOD == pozBarkod).ToList();
                var pozIdDIPB = dipbQuery.Select(a => a.POZ_ID).LastOrDefault();
                



                //Poz No - Döküm Türü
                var query = db2.POZ_KART.Where(a => a.POZ_BARKOD == pozBarkod || a.ID == pozIdDIPB).ToList();

                if(query.Count > 0)
                {
                    ViewBag.dokum = query.Select(a => a.POZ_TURU).LastOrDefault();
                    ViewBag.pozNo = query.Select(a => a.POZ_NO).LastOrDefault();

                    //Proje Bilgisi
                    var projeID = query.Select(a => a.PROJE_ID).LastOrDefault();
                    var projeQuery = db2.PROJE_KART.Where(a => a.ID == projeID).ToList();
                    ViewBag.proje = projeQuery.Select(a => a.PROJE_ADI).LastOrDefault();

                    //Malzeme - M2
                    var pozID = query.Select(a => a.ID).FirstOrDefault();
                    var mq = db2.KALIP_TASARIM_POZ.Where(a => a.POZ_ID == pozID).ToList();
                    var ktID = mq.Select(a => a.KALIP_TASARIM_ID).FirstOrDefault();
                    var mq2 = db2.KALIP_TASARIM_FORM.Where(a => a.ID == ktID).ToList();
                    var mq3 = db2.KALIP_OLUSTUR_FORM.Where(a => a.KALIP_TASARIM_ID == ktID && a.ISLEM_TURU == "KALIP").ToList();
                    ViewBag.malzeme = mq3.Select(a => a.ISLEM_ADI).LastOrDefault();
                    ViewBag.kalıpM2 = mq2.Select(a => a.KALIP_M2).LastOrDefault();

                    //Barkod No - Tarih - Tarama Sayısı
                    var queryBarkod = db2.KALIP_IS_EMRI.Where(a => a.KALIP_TASARIM_ID == ktID && a.PROJE_ID == projeID).ToList();
                    var barkod = queryBarkod.Select(a => a.IS_EMRI_BARKOD).LastOrDefault();
                    ViewBag.barkod = barkod;
                    ViewBag.bugun = DateTime.Now.ToShortDateString();

                    var queryTaramaSayi = db.Tbl_Taramalar.Where(a => a.BarkodT == barkod).ToList();
                    var sayi = queryTaramaSayi.Select(a => a.SayıT).Count();
                    ViewBag.sayi = sayi + 1;
                }

                else
                {
                    var query2 = db2.DOKUM_IS_EMRI_POZ.Where(a => a.DOKUM_POZ_BARKOD == pozBarkod).ToList();                   

                    if(query2.Count > 0)
                    {
                        //Poz ID
                        var pozID = query2.Select(a => a.POZ_ID).LastOrDefault();                    
                        var queryPozKart = db2.POZ_KART.Where(a => a.ID == pozID).ToList();
                        ViewBag.pozNo = queryPozKart.Select(a => a.POZ_NO).LastOrDefault();
                        ViewBag.dokum = queryPozKart.Select(a => a.POZ_TURU).LastOrDefault();

                        //Proje Bilgisi
                        var projeID = query2.Select(a => a.PROJE_ID).LastOrDefault();
                        var projeQuery = db2.PROJE_KART.Where(a => a.ID == projeID).ToList();
                        ViewBag.proje = projeQuery.Select(a => a.PROJE_ADI).LastOrDefault();

                        //Malzeme - M2
                        var mq = db2.KALIP_TASARIM_POZ.Where(a => a.POZ_ID == pozID).ToList();
                        var ktID = mq.Select(a => a.KALIP_TASARIM_ID).FirstOrDefault();
                        var mq2 = db2.KALIP_TASARIM_FORM.Where(a => a.ID == ktID).ToList();
                        var mq3 = db2.KALIP_OLUSTUR_FORM.Where(a => a.KALIP_TASARIM_ID == ktID && a.ISLEM_TURU == "KALIP").ToList();
                        ViewBag.malzeme = mq3.Select(a => a.ISLEM_ADI).LastOrDefault();
                        ViewBag.kalıpM2 = mq2.Select(a => a.KALIP_M2).LastOrDefault();

                        //Barkod No - Tarih - Tarama Sayısı
                        var queryBarkod = db2.KALIP_IS_EMRI.Where(a => a.KALIP_TASARIM_ID == ktID && a.PROJE_ID == projeID).ToList();
                        var barkod = queryBarkod.Select(a => a.IS_EMRI_BARKOD).LastOrDefault();
                        ViewBag.barkod = barkod;
                        ViewBag.bugun = DateTime.Now.ToShortDateString();

                        var queryTaramaSayi = db.Tbl_Taramalar.Where(a => a.BarkodT == barkod).ToList();
                        var sayi = queryTaramaSayi.Select(a => a.SayıT).Count();
                        ViewBag.sayi = sayi + 1;

                    }
                }
                               

            }
            catch (SqlException)
            {
                TempData["errorQuery"] = "Poz barkod numarası eksik veya hatalı!";
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Save(Tbl_Taramalar p1, List<HttpPostedFileBase> images)
        {
            

            try
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (images[i] != null)
                    {
                        byte[] imageBytes = new byte[images[i].ContentLength];
                        images[i].InputStream.Read(imageBytes, 0, images[i].ContentLength);

                        switch (i)
                        {
                            case 0:
                                p1.ResimT1 = imageBytes;
                                break;
                            case 1:
                                p1.ResimT2 = imageBytes;
                                break;
                            case 2:
                                p1.ResimT3 = imageBytes;
                                break;
                            case 3:
                                p1.ResimT4 = imageBytes;
                                break;
                            case 4:
                                p1.ResimT5 = imageBytes;
                                break;
                            case 5:
                                p1.ResimT6 = imageBytes;
                                break;
                            case 6:
                                p1.ResimT7 = imageBytes;
                                break;
                            case 7:
                                p1.ResimT8 = imageBytes;
                                break;
                            case 8:
                                p1.ResimT9 = imageBytes;
                                break;
                            case 9:
                                p1.ResimT10 = imageBytes;
                                break;
                        }
                    }
                }
                //if (image1 != null)
                //{
                //    p1.ResimT1 = new byte[image1.ContentLength];
                //    image1.InputStream.Read(p1.ResimT1, 0, image1.ContentLength);

                //}

                //if (image2 != null)
                //{
                //    p1.ResimT2 = new byte[image2.ContentLength];
                //    image2.InputStream.Read(p1.ResimT2, 0, image2.ContentLength);
                //}

                //if (image3 != null)
                //{
                //    p1.ResimT3 = new byte[image3.ContentLength];
                //    image3.InputStream.Read(p1.ResimT3, 0, image3.ContentLength);
                //}

                //if (image4 != null)
                //{
                //    p1.ResimT4 = new byte[image4.ContentLength];
                //    image4.InputStream.Read(p1.ResimT4, 0, image4.ContentLength);
                //}

                //if (image5 != null)
                //{
                //    p1.ResimT5 = new byte[image5.ContentLength];
                //    image5.InputStream.Read(p1.ResimT5, 0, image5.ContentLength);
                //}

                //if (image6 != null)
                //{
                //    p1.ResimT6 = new byte[image6.ContentLength];
                //    image6.InputStream.Read(p1.ResimT6, 0, image6.ContentLength);
                //}

                //if (image7 != null)
                //{
                //    p1.ResimT7 = new byte[image7.ContentLength];
                //    image7.InputStream.Read(p1.ResimT7, 0, image7.ContentLength);
                //}

                //if (image8 != null)
                //{
                //    p1.ResimT8 = new byte[image8.ContentLength];
                //    image8.InputStream.Read(p1.ResimT8, 0, image8.ContentLength);
                //}

                //if (image9 != null)
                //{
                //    p1.ResimT9 = new byte[image9.ContentLength];
                //    image9.InputStream.Read(p1.ResimT9, 0, image9.ContentLength);
                //}

                //if (image10 != null)
                //{
                //    p1.ResimT10 = new byte[image10.ContentLength];
                //    image10.InputStream.Read(p1.ResimT10, 0, image10.ContentLength);
                //}

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



                TempData["successAdd"] = "Tarama kaydı başarılı!";

            }
            catch (Exception)
            {
                TempData["errorAdd"] = "Bilgiler eksik veya hatalı!";
            }

            return RedirectToAction("AddScanNP", "TaramaEkleNP");

        }

        [HttpGet]
        [Authorize]
        public ActionResult AddScanConcNP()
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
                return RedirectToAction("Dashboard", "HomePage");
            }
        }

        [HttpPost]
        public ActionResult AddScanConcNP(string BarkodT)
        {
            BarkodT = BarkodT.ToUpper();
            Session["BarkodT"] = BarkodT;
            try
            {
                var queryDokum = db2.DOKUM_IS_EMRI_DOKUM.Where(a => a.DOKUM_BARKOD == BarkodT).ToList();

                if(queryDokum.Count > 0)
                {
                    //Poz ID
                    var pozID = queryDokum.Select(a => a.POZ_ID).LastOrDefault();
                    var queryPozKart = db2.POZ_KART.Where(a => a.ID == pozID).ToList();
                    ViewBag.pozNo = queryPozKart.Select(a => a.POZ_NO).LastOrDefault();
                    ViewBag.dokum = queryPozKart.Select(a => a.POZ_TURU).LastOrDefault();
                    ViewBag.panelBarkod = queryDokum.Select(a => a.DOKUM_BARKOD).LastOrDefault();

                    //Proje Bilgisi
                    var projeID = queryDokum.Select(a => a.PROJE_ID).LastOrDefault();
                    var projeQuery = db2.PROJE_KART.Where(a => a.ID == projeID).ToList();
                    ViewBag.proje = projeQuery.Select(a => a.PROJE_ADI).LastOrDefault();

                    //M2
                    ViewBag.panelM2 = queryDokum.Select(a => a.V_URUN_M2).LastOrDefault();

                    //Tarih - Tarama Sayısı
                    ViewBag.today = DateTime.Now.ToShortDateString();

                    var queryTaramaSayi = db.Tbl_Taramalar.Where(a => a.BarkodT == BarkodT).ToList();
                    var sayi = queryTaramaSayi.Select(a => a.SayıT).Count();
                    ViewBag.sayiPanel = sayi + 1;
                }


            }

            catch (Exception)
            {
                TempData["errorQueryC"] = "Barkod numarası eksik veya hatalı!";
            }


            return View();
        }


        
        [HttpPost]
        public ActionResult SaveConc(Tbl_Taramalar p2, HttpPostedFileBase imageP1, HttpPostedFileBase imageP2, HttpPostedFileBase imageP3, HttpPostedFileBase imageP4, HttpPostedFileBase imageP5, HttpPostedFileBase imageP6
            , HttpPostedFileBase imageP7, HttpPostedFileBase imageP8, HttpPostedFileBase imageP9, HttpPostedFileBase imageP10)
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
            p2.BarkodT = Session["BarkodT"].ToString();
            db.Tbl_Taramalar.Add(p2);

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            //db.SaveChanges();
            //TempData["successAddC"] = "Tarama kaydı başarılı!";

            //try
            //{
            //    //if (imageP1 != null)
            //    //{
            //    //    p2.ResimT1 = new byte[imageP1.ContentLength];
            //    //    imageP1.InputStream.Read(p2.ResimT1, 0, imageP1.ContentLength);

            //    //}

            //    //if (imageP2 != null)
            //    //{
            //    //    p2.ResimT2 = new byte[imageP2.ContentLength];
            //    //    imageP2.InputStream.Read(p2.ResimT2, 0, imageP2.ContentLength);
            //    //}

            //    //if (imageP3 != null)
            //    //{
            //    //    p2.ResimT3 = new byte[imageP3.ContentLength];
            //    //    imageP3.InputStream.Read(p2.ResimT3, 0, imageP3.ContentLength);
            //    //}

            //    //if (imageP4 != null)
            //    //{
            //    //    p2.ResimT4 = new byte[imageP4.ContentLength];
            //    //    imageP4.InputStream.Read(p2.ResimT4, 0, imageP4.ContentLength);
            //    //}

            //    //if (imageP5 != null)
            //    //{
            //    //    p2.ResimT5 = new byte[imageP5.ContentLength];
            //    //    imageP5.InputStream.Read(p2.ResimT5, 0, imageP5.ContentLength);
            //    //}

            //    //if (imageP6 != null)
            //    //{
            //    //    p2.ResimT6 = new byte[imageP6.ContentLength];
            //    //    imageP6.InputStream.Read(p2.ResimT6, 0, imageP6.ContentLength);
            //    //}

            //    //if (imageP7 != null)
            //    //{
            //    //    p2.ResimT7 = new byte[imageP7.ContentLength];
            //    //    imageP7.InputStream.Read(p2.ResimT7, 0, imageP7.ContentLength);
            //    //}

            //    //if (imageP8 != null)
            //    //{
            //    //    p2.ResimT8 = new byte[imageP8.ContentLength];
            //    //    imageP8.InputStream.Read(p2.ResimT8, 0, imageP8.ContentLength);
            //    //}

            //    //if (imageP9 != null)
            //    //{
            //    //    p2.ResimT9 = new byte[imageP9.ContentLength];
            //    //    imageP9.InputStream.Read(p2.ResimT9, 0, imageP9.ContentLength);
            //    //}

            //    //if (imageP10 != null)
            //    //{
            //    //    p2.ResimT10 = new byte[imageP10.ContentLength];
            //    //    imageP10.InputStream.Read(p2.ResimT10, 0, imageP10.ContentLength);
            //    //}

            //    //p2.DurumT = "BOŞ";
            //    //db.Tbl_Taramalar.Add(p2);
            //    //db.SaveChanges();
            //    //TempData["successAddC"] = "Tarama kaydı başarılı!";
            //}

            //catch (Exception)
            //{
            //    TempData["errorAddC"] = "Bilgiler eksik veya hatalı!";
            //}

            return RedirectToAction("AddScanConcNP", "TaramaEkleNP");
        }
    }
}