using Microsoft.Ajax.Utilities;
using PortalFibrobeton.Models.Class.UretimSevkiyatRapor;
using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using OfficeOpenXml;
using System.IO;
using System.Net;
using OfficeOpenXml.Table;




namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class UretimSevkiyatRaporController : Controller
    {
        PERA_FIBROEntities dbPera = new PERA_FIBROEntities();
        UretimVeSevkiyatRaporuEntities dbUrSev = new UretimVeSevkiyatRaporuEntities();
        
        

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult HedefEkle()
        {
            return View();
            //var lastWeekStart = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek + 6) % 7 - 6);
            //var lastWeekEnd = lastWeekStart.AddDays(6);

        }
        

        [HttpGet]
        public ActionResult HaftalikUretimSevkiyatRaporu()
        {

            var viewModel = new UretimSevkiyatRaporViewModel
            {
                Hedefler = new List<Hedefler>(),
                Cepheler = new List<PROJE_BLOK_CEPHE>(),
                Siparisler = new List<UretimSevkiyatRaporViewModelItem>(),
                SiparisHafta = new List<UretimSevkiyatRaporViewModelItem>(),
                Dokum = new List<UretimSevkiyatRaporViewModelItem>(),
                DokumTotalTarihsiz = new List<UretimSevkiyatRaporViewModelItem>(),
                Sevkiyat = new List<UretimSevkiyatRaporViewModelItem>(),
                SevkiyatTotalTarihsiz = new List<UretimSevkiyatRaporViewModelItem>(),
            };

            return View(viewModel);
        }

        [HttpPost]

        public ActionResult HaftalikUretimSevkiyatRaporu(DateTime bas, DateTime bit)
        {
            
            var projeID = 112113;

            var siparisR = (from sp in dbPera.SIPARIS_POZ
                            join sf in dbPera.SIPARIS_FORM on sp.SIPARIS_ID equals sf.ID
                            where sf.SIPARIS_TARIH >= bas && sf.SIPARIS_TARIH <= bit && sf.PROJE_ID == projeID
                            select new UretimSevkiyatRaporViewModelItem
                            {
                                ID = (int)sp.ID,
                                Siparis_Miktar = (int)sp.ILK_SIPARIS_MIKTAR,
                                CEPHE_ADI = sp.CEPHE_ADI,
                                BLOK_ADI = sp.BLOK_ADI,
                            })
                            .ToList();


            var siparisTotalSayi = (from sp in dbPera.SIPARIS_POZ
                            join sf in dbPera.SIPARIS_FORM on sp.SIPARIS_ID equals sf.ID
                            where sf.SIPARIS_TARIH <= bit && sf.PROJE_ID == projeID
                            select new UretimSevkiyatRaporViewModelItem
                            {
                                ID = (int)sp.ID,
                                Siparis_Miktar = (int)sp.ILK_SIPARIS_MIKTAR,
                                CEPHE_ADI = sp.CEPHE_ADI,
                                BLOK_ADI = sp.BLOK_ADI,
                            })
                            .ToList();



            var dokumR = (from d in dbPera.KALIP_IS_EMRI_DOKUM
                          join p in dbPera.SIPARIS_POZ on new { d.PROJE_ID, d.POZ_NO } equals new { p.PROJE_ID, p.POZ_NO }
                          where d.DOKUM_TARIH >= bas && d.DOKUM_TARIH <= bit && p.PROJE_ID == projeID && d.POZ_ID == p.ID && d.DOKULDU_YAPAN != null
                          select new UretimSevkiyatRaporViewModelItem
                          {
                              ID = (int)d.ID,
                              Poz_No = d.POZ_NO,
                              DOKUM_TARIH = (DateTime)d.DOKUM_TARIH,
                              DOKUM_BARKOD = d.DOKUM_BARKOD,
                              BLOK_ADI = p.BLOK_ADI,
                              CEPHE_ADI = p.CEPHE_ADI

                          })
                          .ToList();

            var dokumTotalSayi = (from d in dbPera.KALIP_IS_EMRI_DOKUM
                                  join p in dbPera.SIPARIS_POZ on new { d.PROJE_ID, d.POZ_NO } equals new { p.PROJE_ID, p.POZ_NO }
                                  where p.PROJE_ID == projeID && d.POZ_ID == p.ID && d.DOKULDU_YAPAN != null && d.DOKUM_TARIH <= bit
                                  select new UretimSevkiyatRaporViewModelItem
                                  {
                                      ID = (int)d.ID,
                                      Poz_No = d.POZ_NO,
                                      DOKUM_TARIH = (DateTime)d.DOKUM_TARIH,
                                      DOKUM_BARKOD = d.DOKUM_BARKOD,
                                      BLOK_ADI = p.BLOK_ADI,
                                      CEPHE_ADI = p.CEPHE_ADI

                                  })
                          .ToList();


            var sevkiyatR = (from p in dbPera.SIPARIS_POZ
                             join sa in dbPera.SEVK_ALT on new { p.PROJE_ID, p.POZ_NO } equals new { sa.PROJE_ID, sa.POZ_NO }
                             join sf in dbPera.SEVK_FIS on sa.SEVK_ID equals sf.ID
                             where sf.OLUSTURMA_TARIH >= bas && sf.OLUSTURMA_TARIH <= bit && sa.PROJE_ID == projeID
                             select new UretimSevkiyatRaporViewModelItem
                             {
                                 ID = (int)p.ID,
                                 Poz_No = p.POZ_NO,
                                 SEVK_TARIH = (DateTime)sf.OLUSTURMA_TARIH,
                                 Sevk_Miktar = (int)sa.SEVK_EDILEN_MIKTAR,
                                 BLOK_ADI = p.BLOK_ADI,
                                 CEPHE_ADI = p.CEPHE_ADI
                             })
                             .ToList();

            var sevkiyatTotalSayi = (from p in dbPera.SIPARIS_POZ
                                     join sa in dbPera.SEVK_ALT on new { p.PROJE_ID, p.POZ_NO } equals new { sa.PROJE_ID, sa.POZ_NO }
                                     join sf in dbPera.SEVK_FIS on sa.SEVK_ID equals sf.ID
                                     where sa.PROJE_ID == projeID && sf.OLUSTURMA_TARIH <= bit
                                     select new UretimSevkiyatRaporViewModelItem
                                     {
                                         ID = (int)p.ID,
                                         Poz_No = p.POZ_NO,
                                         SEVK_TARIH = (DateTime)sf.OLUSTURMA_TARIH,
                                         Sevk_Miktar = (int)sa.SEVK_EDILEN_MIKTAR,
                                         BLOK_ADI = p.BLOK_ADI,
                                         CEPHE_ADI = p.CEPHE_ADI
                                     })
                             .ToList();



            var viewModel = new UretimSevkiyatRaporViewModel
            {
                //Hedefler
                Hedefler = dbUrSev.Hedefler.SqlQuery("SELECT * FROM Hedefler WHERE ProjeID = 112113 AND DATEPART(ISO_WEEK, HedefTarih) = DATEPART(ISO_WEEK, GETDATE())").ToList(),

                //Cepheler Adet-m2
                Cepheler = dbPera.PROJE_BLOK_CEPHE.Where(a => a.PROJE_ID == 112113).ToList(),

                //Sipariş
                Siparisler = siparisTotalSayi.ToList(),
                SiparisHafta = siparisR.ToList(),

                //Döküm
                Dokum = dokumR.ToList(),
                DokumTotalTarihsiz = dokumTotalSayi.ToList(),

                //Sevkiyat
                Sevkiyat = sevkiyatR.ToList(),
                SevkiyatTotalTarihsiz = sevkiyatTotalSayi.ToList()



            };

            return View(viewModel);
        }

        public MemoryStream ExcelRaporuOlustur(UretimSevkiyatRaporViewModel viewModel)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage()) 
            {
                var ws = package.Workbook.Worksheets.Add("Report");

                // Başlık satırları
                ws.Cells[1, 1].Value = "Building Facades";
                ws.Cells[1, 2].Value = "Total Panel Quantity";
                ws.Cells[1, 3, 1, 6].Merge = true;
                ws.Cells[1, 3].Value = "Ordered";
                ws.Cells[1, 7].Value = "Hold";
                ws.Cells[1, 8, 1, 11].Merge = true;
                ws.Cells[1, 8].Value = "Casting";
                ws.Cells[1, 12, 1, 15].Merge = true;
                ws.Cells[1, 12].Value = "Shipped";

                // Alt başlık satırları
                ws.Cells[2, 3].Value = "Total up to this week";
                ws.Cells[2, 4].Value = "This week planing";
                ws.Cells[2, 5].Value = "This week realized";
                ws.Cells[2, 6].Value = "Remaining";
                ws.Cells[2, 7].Value = "Piece";
                ws.Cells[2, 8].Value = "Total up to this week";
                ws.Cells[2, 9].Value = "This week planing";
                ws.Cells[2, 10].Value = "This week realized";
                ws.Cells[2, 11].Value = "Remaining";
                ws.Cells[2, 12].Value = "Total up to this week";
                ws.Cells[2, 13].Value = "This week planing";
                ws.Cells[2, 14].Value = "This week realized";
                ws.Cells[2, 15].Value = "Remaining";

                var stream = new MemoryStream();
                package.SaveAs(stream);
                return stream;
            }
        }

        public void SendReportEmail(MemoryStream excelStream)
        {
            var fromAdress = new MailAddress("ozan.yukseldemir@fibrobeton.com.tr", "Ozan Yükseldemir");
            var toAdress = new MailAddress("oziyukseldemir@gmail.com", "To Name");
            const string subject = "Haftalık Üretim Raporu";
            const string body = "Merhaba, haftalık üretim sevkiyat raporunuz ektedir";

            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAdress.Address, "Blackelite19")
            };

            using (var message = new MailMessage(fromAdress, toAdress)
            {
                Subject = subject,
                Body = body
            })
            {
                excelStream.Position = 0;
                message.Attachments.Add(new Attachment(excelStream, "haftalik_uretim_sevkiyat_raporu.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                smtp.Send(message);
            }
        }


        public JsonResult InsertHedefler(List<Hedefler> hedefler)
        {
            DateTime today = new DateTime();
            today = DateTime.Today;
            foreach(Hedefler s in hedefler)
            {
                s.HedefTarih = today;
                dbUrSev.Hedefler.Add(s);
            }
            int kaydet = dbUrSev.SaveChanges();
            return Json(kaydet);
        }
    }

    
}