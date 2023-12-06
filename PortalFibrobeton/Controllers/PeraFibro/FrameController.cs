using Microsoft.Ajax.Utilities;
using PortalFibrobeton.Models.Class;
using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Attributelar;


namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class FrameController : Controller
    {
        string path;
        PERA_FIBROEntities db = new PERA_FIBROEntities();
        
        // GET: Frame
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FrameCizim()
        {
            var query2 = from a in db.ARSIV_DOKUMAN.SqlQuery("SELECT * FROM ARSIV_DOKUMAN where OLUSTURULMA_TARIH = Dateadd(day,datediff(day,2,GETDATE()),0)")
                         select a;

            var model = new TeknikResimler
            {
                ProjeList = (from a in db.PROJE_KART
                            orderby a.PROJE_ADI ascending
                            select a).ToList()
                
            };

            return View(model);
        }

        [HttpPost]
        [MultipleButtonResults(Name = "action", Argument = "barkod")]
        public ActionResult FrameCizim(TeknikResimler model,string barkod)
        {
            int projeID;
            string pozNo;
            int KalipIsID;
            string IsEmriBarkod;
            string PozAdi;         
            try
            {
                if(barkod != null)
                {
                    //İş Emri Numarasına Göre
                    var queryIsEmriBarkod = db.KALIP_IS_EMRI.Where(a => a.IS_EMRI_BARKOD == barkod).ToList();

                    if (queryIsEmriBarkod.Count > 0)
                    {
                        KalipIsID = (int)queryIsEmriBarkod.Select(a => a.ID).LastOrDefault();
                        var queryKalipIsPoz = db.KALIP_IS_EMRI_POZ.Where(a => a.KALIP_IS_ID == KalipIsID).ToList();
                        projeID = (int)queryKalipIsPoz.Select(a => a.PROJE_ID).LastOrDefault();
                        List<string> PozNos = new List<string>();
                        foreach (var i in queryKalipIsPoz.Select(a => a.POZ_NO).ToList())
                        {
                            PozNos.Add(i);
                        }

                        var queryPozSiparis = db.SIPARIS_POZ.Where(a => a.PROJE_ID == projeID && PozNos.Contains(a.POZ_NO)).ToList();
                        List<string> PozAdiList = new List<string>();
                        foreach (var i in queryPozSiparis.Select(a => a.POZ_ADI).ToList())
                        {
                            PozAdiList.Add(i);
                        }

                        var queryArsiv = db.ARSIV_DOKUMAN.Where(a => PozAdiList.Contains(a.KOD) || a.KOD == barkod).ToList();
                        model.ArsivListe = queryArsiv.Where(a => a.DOSYA_YOLU.Substring(a.DOSYA_YOLU.Length - 3) != "3dm").ToList();
                        model.ProjeList = (from a in db.PROJE_KART
                                           orderby a.PROJE_ADI ascending
                                           select a).ToList();
                        return View(model);

                    }


                    //Poz Barkoda Göre
                    var queryPozBarkod = db.KALIP_IS_EMRI_POZ.Where(a => a.POZ_BARKOD == barkod).ToList();

                    if (queryPozBarkod.Count > 0)
                    {
                        projeID = (int)queryPozBarkod.Select(a => a.PROJE_ID).LastOrDefault();
                        pozNo = (string)queryPozBarkod.Select(a => a.POZ_NO).LastOrDefault();
                        KalipIsID = (int)queryPozBarkod.Select(a => a.KALIP_IS_ID).LastOrDefault();

                        //İş Emri Barkod
                        var queryKalipIsEmri = db.KALIP_IS_EMRI.Where(a => a.ID == KalipIsID).ToList();
                        IsEmriBarkod = (string)queryKalipIsEmri.Select(a => a.IS_EMRI_BARKOD).LastOrDefault();

                        //Poz Adı
                        var querySiparisPoz = db.SIPARIS_POZ.Where(a => a.PROJE_ID == projeID && a.POZ_NO == pozNo).ToList();
                        PozAdi = (string)querySiparisPoz.Select(a => a.POZ_ADI).LastOrDefault();

                        var queryArsiv = db.ARSIV_DOKUMAN.Where(a => a.KOD == PozAdi || a.KOD == IsEmriBarkod).ToList();
                        var pdfSelector = queryArsiv.Where(a => a.DOSYA_YOLU.Substring(a.DOSYA_YOLU.Length - 3) != "3dm").ToList();
                        model.ArsivListe = pdfSelector.ToList();
                        model.ProjeList = (from a in db.PROJE_KART
                                           orderby a.PROJE_ADI ascending
                                           select a).ToList();

                        return View(model);


                    }

                    //Döküm Barkoduna Göre

                    var queryDokumBarkod = db.KALIP_IS_EMRI_DOKUM.Where(a => a.DOKUM_BARKOD == barkod).ToList();

                    if (queryDokumBarkod.Count > 0)
                    {
                        projeID = (int)queryDokumBarkod.Select(a => a.PROJE_ID).LastOrDefault();
                        pozNo = queryDokumBarkod.Select(a => a.POZ_NO).LastOrDefault();

                        var querySiparis = db.SIPARIS_FORM.Where(a => a.PROJE_ID == projeID).Select(a => a.SIPARIS_BARKOD).ToList();

                        List<string> SiparisBarkod = new List<string>();
                        foreach (var i in querySiparis)
                        {
                            SiparisBarkod.Add(i);
                        }


                        var queryArsiv = db.ARSIV_DOKUMAN.Where(a => SiparisBarkod.Contains(a.KOD)).ToList();

                        var filtrePoz = queryArsiv.Where(a => a.ARSIV_ADI.Contains(pozNo)).ToList();

                        var pdfSelector = filtrePoz.Where(a => a.DOSYA_YOLU.Substring(a.DOSYA_YOLU.Length - 3) != "3dm").ToList();
                        model.ArsivListe = pdfSelector.ToList();
                        model.ProjeList = (from a in db.PROJE_KART
                                           orderby a.PROJE_ADI ascending
                                           select a).ToList();

                        return View(model);



                    }
                }
                

            }
            catch (SqlException)
            {
                TempData["errorFrameCizim"] = "Bu poza ait çizim bulunamadı!";
                
            }
            return View(model);
        }


        [HttpPost]
        [MultipleButtonResults(Name = "action", Argument = "proje")]
        public ActionResult GetProje(TeknikResimler model, int projeID)
        {
            model.ProjeList = (from a in db.PROJE_KART
                               orderby a.PROJE_ADI ascending
                               select a).ToList();

            var querySiparis = db.SIPARIS_FORM.Where(a => a.PROJE_ID == projeID).Select(a => a.SIPARIS_BARKOD).ToList();

            List<string> SiparisBarkod = new List<string>();
            foreach (var i in querySiparis)
            {
                SiparisBarkod.Add(i);
            }

            var queryArsiv = db.ARSIV_DOKUMAN.Where(a => SiparisBarkod.Contains(a.KOD)).ToList();
            var pdfSelector = queryArsiv.Where(a => a.DOSYA_YOLU.Substring(a.DOSYA_YOLU.Length - 3) != "3dm").ToList();
            model.ArsivListe = pdfSelector.ToList();
            model.ProjeList = (from a in db.PROJE_KART
                               orderby a.PROJE_ADI ascending
                               select a).ToList();

            return View(model);
        }


        public ActionResult GetPdf(string refID)
        {

            string SQL = "SELECT * FROM ARSIV_DOKUMAN where REF_NO = @p2";
            SqlParameter parDok = new SqlParameter("@p2", refID);
            object[] objDok = new object[] { parDok };
            var query2 = db.ARSIV_DOKUMAN.SqlQuery(SQL, objDok);
            path = query2.Select(a => a.DOSYA_YOLU).LastOrDefault();
            path = path.Substring(2);
            string pdfPath = "portal.fibrobeton.com.tr/frame" + path;

            return Redirect(pdfPath);
        }

        public ActionResult Download(string refID)
        {
            string SQL = "SELECT * FROM ARSIV_DOKUMAN where REF_NO = @p3";
            SqlParameter parDok = new SqlParameter("@p3", refID);
            object[] objDoku = new object[] { parDok };
            var query = db.ARSIV_DOKUMAN.SqlQuery(SQL, objDoku);
            path = query.Select(a => a.DOSYA_YOLU).LastOrDefault();
            path = path.Substring(2);
            string downloadPath = "//193.100.100.6/pera_dokuman/" + path;
            var fileStream = new FileStream(downloadPath, FileMode.Open, FileAccess.Read);
            return File(fileStream, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(downloadPath));
        }

    }
}