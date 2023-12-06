using Microsoft.Ajax.Utilities;
using PortalFibrobeton.Models.Class;
using PortalFibrobeton.Models.Class.HammaddeTakip;
using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;


namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class HammaddeController : Controller
    {
        PERA_FIBROEntities dbPera = new PERA_FIBROEntities();
        // GET: Hammadde
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult HammaddeTakip()
        {
            
            return View();
        }

        public ActionResult HammaddeTakip(DateTime bas, DateTime bit)
        {

            DateTime gecmisTarih = bas.AddDays(-100);


            //Üretim Raporu 
            var uretimRaporuListe = from d in dbPera.KALIP_IS_EMRI_DOKUM
                                    join s in dbPera.SIPARIS_POZ on d.POZ_ID equals s.ID
                                    join p in dbPera.PROJE_KART on d.PROJE_ID equals p.ID
                                    where d.DOKUM_TARIH >= gecmisTarih && d.DOKUM_TARIH <= bas
                                    group new { p, d, s } by new { p.PROJE_ADI, p.ID } into g
                                    select new uretimRaporuModelHammadde
                                    {
                                        ProjeAdi = g.Key.PROJE_ADI,
                                        Count = g.Count(),
                                        TotalBirimKG = (double)g.Sum(x => x.d.BIRIM_KG),
                                        TotalDokumM2 = (double)g.Sum(x => x.s.DOKUM_M2),
                                        ProjeID = g.Key.ID
                                        
                                    };


            //Frame Ağırlık
            var queryFrame = (from d in dbPera.KALIP_IS_EMRI_DOKUM
                             join f in dbPera.KALIP_FRAME_TEDARIK on d.POZ_ID equals f.POZ_ID
                             join p in dbPera.PROJE_KART on d.PROJE_ID equals p.ID
                             where d.DOKUM_TARIH >= gecmisTarih && d.DOKUM_TARIH <= bas
                             select new
                             {
                                 ProjeAdi = p.PROJE_ADI,
                                 ID = d.ID,
                                 FrameAgirlik = (double)f.BIRIM_KG,
                                 ProjeID = p.ID
                             }).Distinct();


            var groupedFrame = (from f in queryFrame
                                group f by f.ProjeID into g
                                select new frameDokumHammaddeRaporu
                                {
                                    ProjeID = g.Key,
                                    FrameAgirlik = g.Sum(x => x.FrameAgirlik),
                                }).ToList();

            //Planan Dökümler
            var plananDokumListe = from d in dbPera.KALIP_IS_EMRI_DOKUM
                                   join s in dbPera.SIPARIS_POZ on d.POZ_ID equals s.ID
                                   join p in dbPera.PROJE_KART on d.PROJE_ID equals p.ID
                                   where d.DOKUM_TARIH >= bas && d.DOKUM_TARIH <= bit
                                   select new planlananDokumRaporuHammadde
                                   {
                                       PozNo = s.POZ_NO,
                                       DokumTarih = d.DOKUM_TARIH,
                                       ProjeAdi = p.PROJE_ADI,
                                       DokumM2 = s.DOKUM_M2,
                                       ProjeID = p.ID

                                   };


            var birlesmisListe = (from uretim in uretimRaporuListe
                                  join planlanan in plananDokumListe on uretim.ProjeID equals planlanan.ProjeID
                                  select new
                                  {
                                      uretim,
                                      planlanan
                                  }).AsEnumerable()
                      .Select(x => new birlesmisHammaddeRaporu
                      {
                          ProjeAdi = x.uretim.ProjeAdi,
                          PozNo = x.planlanan.PozNo,
                          KalıpM2 = x.planlanan.DokumM2,
                          TahminiKutle = x.planlanan.DokumM2 * ((x.uretim.TotalBirimKG - (groupedFrame.FirstOrDefault(g => g.ProjeID == x.uretim.ProjeID)?.FrameAgirlik ?? 0)) / x.uretim.TotalDokumM2),
                          TahminiFrameKutle = x.planlanan.DokumM2 * (groupedFrame.FirstOrDefault(g => g.ProjeID == x.uretim.ProjeID)?.FrameAgirlik / x.uretim.TotalDokumM2),
                          ToplamKutle = x.planlanan.DokumM2 * (((x.uretim.TotalBirimKG - (groupedFrame.FirstOrDefault(g => g.ProjeID == x.uretim.ProjeID)?.FrameAgirlik ?? 0)) / x.uretim.TotalDokumM2) + (groupedFrame.FirstOrDefault(g => g.ProjeID == x.uretim.ProjeID)?.FrameAgirlik / x.uretim.TotalDokumM2)),
                          TamKovaSayisi = x.planlanan.DokumM2 * (x.uretim.TotalBirimKG / x.uretim.TotalDokumM2) / 118
                      }).ToList();

            

            return View(birlesmisListe.ToList());
        }

    }
}