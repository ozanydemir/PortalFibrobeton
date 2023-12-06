using Microsoft.Ajax.Utilities;
using PortalFibrobeton.Attributelar;
using PortalFibrobeton.Models.Class;
using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Xml.Linq;

namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class ArsivDokumanController : Controller
    {
        PERA_FIBROEntities db = new PERA_FIBROEntities();
        PERA_FIBRO_ULTIMATEEntities db2 = new PERA_FIBRO_ULTIMATEEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cizimler()
        {
            var model = new TeknikResimler
            {
                ArsivListeNP = new List<DOKUMAN_YONETIM_LOG>(),
                ProjeListNP = (from a in db2.PROJE_KART
                               orderby a.PROJE_ADI ascending
                               select a).ToList()
            };


            return View(model);
        }


        [HttpPost]
        [MultipleButtonResults(Name = "action", Argument = "barkod")]
        public ActionResult Cizimler(string barkod,TeknikResimler model)
        {
            var kalipTasarimID = 0;
            var kalipIsID = 0;
            
            List<int> pozIDS = new List<int>();

            try
            {
                
                if (barkod != null)
                {
                    //Poz Barkod
                    var queryPozKart = db2.POZ_KART.Where(a => a.POZ_BARKOD == barkod).ToList();
                    if(queryPozKart.Count > 0)
                    {
                        var queryPozKartID = queryPozKart.Select(a => a.ID).LastOrDefault();

                        //var kalipTasarimQuery = db2.KALIP_TASARIM_POZ.Where(a => a.POZ_ID == queryPozKartID).ToList();
                        //kalipTasarimID = (int)kalipTasarimQuery.Select(a => a.KALIP_TASARIM_ID).LastOrDefault();
                        var dokumanYonetim = db2.DOKUMAN_YONETIM_LOG.Where(a => a.VERI_ID == queryPozKartID).ToList();

                        var pdfSelector = dokumanYonetim.Where(a => a.DOSYA_ADI.Substring(a.DOSYA_ADI.Length - 3) != "3dm").ToList();
                        model.ArsivListeNP = pdfSelector.ToList();


                        return View(model);
                    }

                    //İş Emri Numarası
                    var queryIsEmri = db2.KALIP_IS_EMRI.Where(a => a.IS_EMRI_BARKOD == barkod).ToList();
                    if(queryIsEmri.Count > 0)
                    {
                        //KTID Çek
                        kalipTasarimID = (int)queryIsEmri.Select(a => a.KALIP_TASARIM_ID).LastOrDefault();

                        //Poz Idleri çek listeye yazdır
                        var queryPozIDler = db2.KALIP_TASARIM_POZ.Where(a => a.KALIP_TASARIM_ID == kalipTasarimID).Select(a => a.POZ_ID).ToList();
                        foreach(var i in queryPozIDler)
                        {
                            pozIDS.Add((int)i);
                        }

                        var dokumanYonetim = db2.DOKUMAN_YONETIM_LOG.Where(a => a.VERI_ID == kalipTasarimID || pozIDS.Contains((int)a.VERI_ID)).ToList();

                        var pdfSelector = dokumanYonetim.Where(a => a.DOSYA_ADI.Substring(a.DOSYA_ADI.Length - 3) != "3dm").ToList();
                        model.ArsivListeNP = pdfSelector.ToList();

                        return View(model);
                    }

                    //Döküm İş Emri Numarası - DIE
                    var queryDokumIsEmri = db2.DOKUM_IS_EMRI.Where(a => a.DOKUM_IS_EMRI_NO == barkod).ToList();
                    if(queryDokumIsEmri.Count > 0)
                    {
                        //KIIS Çek
                        kalipIsID = (int)queryDokumIsEmri.Select(a => a.KALIP_IS_ID).LastOrDefault();
                        var queryIsEmriD = db2.KALIP_IS_EMRI.Where(a => a.ID == kalipIsID).ToList();
                        
                        //İş Emrine Göre Sorguya Devam Et
                        if(queryIsEmriD.Count > 0)
                        {
                            //KTID Çek
                            kalipTasarimID = (int)queryIsEmriD.Select(a => a.KALIP_TASARIM_ID).LastOrDefault();

                            //Poz Idleri çek listeye yazdır
                            var queryPozIDler = db2.KALIP_TASARIM_POZ.Where(a => a.KALIP_TASARIM_ID == kalipTasarimID).Select(a => a.POZ_ID).ToList();
                            foreach (var i in queryPozIDler)
                            {
                                pozIDS.Add((int)i);
                            }

                            var dokumanYonetim = db2.DOKUMAN_YONETIM_LOG.Where(a => a.VERI_ID == kalipTasarimID || pozIDS.Contains((int)a.VERI_ID)).ToList();

                            var pdfSelector = dokumanYonetim.Where(a => a.DOSYA_ADI.Substring(a.DOSYA_ADI.Length - 3) != "3dm").ToList();
                            model.ArsivListeNP = pdfSelector.ToList();

                            return View(model);
                        }

                    }

                    //Döküm Barkoduna Göre
                    var queryDokumBarkod = db2.DOKUM_IS_EMRI_DOKUM.Where(a => a.DOKUM_BARKOD == barkod).ToList();
                    if (queryDokumBarkod.Count > 0)
                    {
                        //KIIS Çek
                        kalipIsID = (int)queryDokumBarkod.Select(a => a.KALIP_IS_ID).LastOrDefault();
                        var queryIsEmriD = db2.KALIP_IS_EMRI.Where(a => a.ID == kalipIsID).ToList();

                        //İş Emrine Göre Sorguya Devam Et
                        if(queryIsEmriD.Count > 0)
                        {
                            //KTID Çek
                            kalipTasarimID = (int)queryIsEmriD.Select(a => a.KALIP_TASARIM_ID).LastOrDefault();

                            //Poz Idleri çek listeye yazdır
                            var queryPozIDler = db2.KALIP_TASARIM_POZ.Where(a => a.KALIP_TASARIM_ID == kalipTasarimID).Select(a => a.POZ_ID).ToList();
                            foreach (var i in queryPozIDler)
                            {
                                pozIDS.Add((int)i);
                            }

                            var dokumanYonetim = db2.DOKUMAN_YONETIM_LOG.Where(a => a.VERI_ID == kalipTasarimID || pozIDS.Contains((int)a.VERI_ID)).ToList();

                            var pdfSelector = dokumanYonetim.Where(a => a.DOSYA_ADI.Substring(a.DOSYA_ADI.Length - 3) != "3dm").ToList();
                            model.ArsivListeNP = pdfSelector.ToList();

                            return View(model);
                        }

                        return View(model);

                    }

                }            
            }
            catch (SqlException)
            {
                TempData["errorFrameCizimNP"] = "Bu poza ait çizim bulunamadı!";
            }

            return View(model);

        }

        //Klasör Olarak Yüklenen Dosyalar İçin
        public List<string> DosyalariBul(string klasorYolu)
        {

            var bulunanDosyalar = new List<string>();
            string[] uzantilar = new string[] { "*.pdf", "*.jpg", "*.jpeg", "*.dwg", "*.xlsx", "*.txt" };

            Parallel.ForEach(uzantilar, (uzanti) =>
            {
                bulunanDosyalar.AddRange(Directory.GetFiles(klasorYolu, uzanti, SearchOption.AllDirectories));
            });

            return bulunanDosyalar.ToList();
        }

        [HttpPost]
        [MultipleButtonResults(Name = "action", Argument = "proje")]
        public ActionResult GetProje(int projeID, string pozNo)
        {
            List<int> pozIDs = new List<int>();
            List<int> ktIDs = new List<int>();
            List<string> dosyalar;
            List<string> filtrelenmisPozlar = new List<string>();

            //Projeler
            var projeListNP = (from a in db2.PROJE_KART
                             orderby a.PROJE_ADI ascending
                             select a).ToList();

            var ktPozIDQuery = db2.KALIP_TASARIM_POZ.Where(a => a.PROJE_ID == projeID).Select(a => a.POZ_ID).ToList();
            var ktIDSQuery = db2.KALIP_IS_EMRI.Where(a => a.PROJE_ID == projeID).Select(a => a.KALIP_TASARIM_ID).ToList();

            //Kalıp Tasarım Poz ID'ye göre
            foreach (var i in ktPozIDQuery)
            {
                pozIDs.Add((int)i);
            }
           
            //Kalıp Tasarım ID'ye göre Poz IDleri
            foreach(var i in ktIDSQuery)
            {
                ktIDs.Add((int)i);
            }

            //Klasör Olarak Yüklenen Dosyalar İçin Kod
            var klasorYolu = db2.DOKUMAN_YONETIM_LOG.Where(a => a.VERI_ID == projeID).Select(a => a.KLASOR_YOLU).ToList().LastOrDefault();
            if(!string.IsNullOrEmpty(klasorYolu))
            {
                dosyalar = DosyalariBul(klasorYolu.ToString());
            }
            else
            {
                dosyalar = new List<string>();
            }

            if (!string.IsNullOrEmpty(pozNo))
            {
                foreach(var dosya in dosyalar)
                {
                    if (Path.GetFileName(dosya).Contains(pozNo))
                    {
                        filtrelenmisPozlar.Add(dosya);
                    }
                }
            }

            //Diğer Bilgilere Göre Dosyalar
            var queryProje = db2.DOKUMAN_YONETIM_LOG.Where(a => a.VERI_ID == projeID || pozIDs.Contains((int)a.VERI_ID) || ktIDs.Contains((int)a.VERI_ID)).ToList();
            var pdfSelector = queryProje.Where(a => a.DOSYA_ADI.Length >= 3 && a.DOSYA_ADI.Substring(a.DOSYA_ADI.Length - 3) != "3dm" && !a.DOSYA_ADI.StartsWith("20")).ToList();

            var viewModel = new TeknikResimler
            {
                ArsivListeNP = pdfSelector,
                AltKlasorlerList = filtrelenmisPozlar,
                ProjeListNP = projeListNP
            };

            return View(viewModel);
        }


        public ActionResult Download(string refID)
        {
            //string dosyaYolu;
            string klasorYolu;
            string dosyaAdi;
            var query = db2.DOKUMAN_YONETIM_LOG.Where(a => a.REFERANS_NO == refID).ToList();
            dosyaAdi = query.Select(a => a.DOSYA_ADI).LastOrDefault();
            klasorYolu = query.Select(a => a.KLASOR_YOLU).LastOrDefault();

            string downloadPath = klasorYolu + "/" + dosyaAdi;
            var fileStream = new FileStream(downloadPath, FileMode.Open, FileAccess.Read);

            
            return File(fileStream, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(downloadPath));


        }

        public ActionResult DownloadProje(string filePath)
        {
            if(System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath,FileMode.Open, FileAccess.Read);
                var mimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                var fileName = Path.GetFileName(filePath);
                return File(fileStream,mimeType, fileName);
            }
            else
            {
                // Dosya mevcut değilse kullanıcıya bir hata mesajı göster
                return HttpNotFound("Dosya bulunamadı.");
            }
        }
    }
}