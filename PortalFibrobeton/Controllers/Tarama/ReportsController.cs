using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using PortalFibrobeton.Models.Entity;
using PortalFibrobeton.Models.Class.Tarama;
using System.Globalization;

namespace PortalFibrobeton.Controllers
{

    public class ReportsController : Controller
    {
        //Yüzde Hesap Formül
        double YuzdeHesap(double a, double b)
        {
            if (a != 0)
            {
                double sonuc = ((a / b) * 100);
                sonuc = Math.Round(sonuc, 0);
                return sonuc;
            }
            else
            {
                return 0;
            }
        }

        TaramaEntities db = new TaramaEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kalıphaneler()
        {
            
            var sqlActivity = (from a in db.Tbl_Taramalar.SqlQuery("SELECT TOP 10 * FROM TBL_TARAMALAR where MalzemeT != 'BETON' order by IDT desc").ToList()
                               select a);

            //Dünün Verileri
            var dun = DateTime.Today.AddDays(-1);
            var sqlSorguDun = db.Tbl_Taramalar.Where(a => a.TarihT == dun).ToList();

            //Dün Tarama Hata Sayısı
            var yuzdeler6Dun = new List<double>();
            var yuzdeler9Dun = new List<double>();
            var yuzdelerTopDun = new List<double>();
            var DunlikToplam = sqlSorguDun.Select(a => a.IDT).Count();

            for (int i = 0; i <= 10; i++)
            {
                var redSayi9Dun = sqlSorguDun.Count(a => a.DurumT == "RED" && a.SayıT == i && a.TeslimT == "HOL-9");
                var redSayi6Dun = sqlSorguDun.Count(a => a.DurumT == "RED" && a.SayıT == i && a.TeslimT == "HOL-6");
                var redSayiTopDun = sqlSorguDun.Count(a => a.DurumT == "RED" && a.SayıT == i);
                var yuzde6Dun = YuzdeHesap(redSayi6Dun, DunlikToplam);
                var yuzde9Dun = YuzdeHesap(redSayi6Dun, DunlikToplam);
                var yuzdeTopDun = YuzdeHesap(redSayi6Dun, DunlikToplam);
                yuzdeler6Dun.Add(yuzde6Dun);
                yuzdeler9Dun.Add(yuzde9Dun);
                yuzdelerTopDun.Add(yuzdeTopDun);

            }
            ViewBag.DunHataYuzdelerTop = yuzdelerTopDun;
            ViewBag.DunHataYuzdeler6 = yuzdeler6Dun;
            ViewBag.DunHataYuzdeler9 = yuzdeler9Dun;


            //---------------------------------------------------------------------------------------------------------------------------------//

            // Haftalık Veriler
            var baslangicTarihi = DateTime.Today.AddDays(-6);
            var bitisTarihi = DateTime.Today;

            var sqlSorguDash = db.Tbl_Taramalar
                .Where(a => a.TarihT >= baslangicTarihi && a.TarihT <= bitisTarihi)
                .ToList();

            var grupDataSql = Enumerable.Range(0, 7)
                .Select(offset => baslangicTarihi.AddDays(offset))
                .Select(tarih => new
                {
                    Tarih = tarih,
                    Gun = tarih.ToString("dddd", new CultureInfo("tr-TR")),
                    HOL6 = sqlSorguDash.Count(a => a.TarihT.Date == tarih.Date && a.TeslimT == "HOL-6"),
                    HOL9 = sqlSorguDash.Count(a => a.TarihT.Date == tarih.Date && a.TeslimT == "HOL-9"),
                    TASERON = sqlSorguDash.Count(a => a.TarihT.Date == tarih.Date && a.TeslimT == "TAŞERON")
                })
                .ToList();

            ViewBag.HaftalikTaramaChart = grupDataSql;


            //Haftalık Tarama Hata Sayısı
            var yuzdeler6Hafta = new List<double>();
            var yuzdeler9Hafta = new List<double>();
            var yuzdelerTopHafta = new List<double>();
            var haftalikToplam = sqlSorguDash.Select(a => a.IDT).Count();

            for (int i = 0; i <= 10; i++)
            {
                var redSayi9Hafta = sqlSorguDash.Count(a => a.DurumT == "RED" && a.SayıT == i && a.TeslimT == "HOL-9");
                var redSayi6Hafta = sqlSorguDash.Count(a => a.DurumT == "RED" && a.SayıT == i && a.TeslimT == "HOL-6");
                var redSayiTopHafta = sqlSorguDash.Count(a => a.DurumT == "RED" && a.SayıT == i);
                var yuzde6Hafta = YuzdeHesap(redSayi6Hafta, haftalikToplam);
                var yuzde9Hafta = YuzdeHesap(redSayi9Hafta, haftalikToplam);
                var yuzdeTopHafta = YuzdeHesap(redSayiTopHafta, haftalikToplam);
                yuzdeler6Hafta.Add(yuzde6Hafta);
                yuzdeler9Hafta.Add(yuzde9Hafta);
                yuzdelerTopHafta.Add(yuzdeTopHafta);

            }
            ViewBag.HaftalikHataYuzdelerTop = yuzdelerTopHafta;
            ViewBag.HaftalikHataYuzdeler6 = yuzdeler6Hafta;
            ViewBag.HaftalikHataYuzdeler9 = yuzdeler9Hafta;

            //---------------------------------------------------------------------------------------------------------------------------------//

            //Bugün Kayıtları
            var bugun = DateTime.Today;
            var sqlSorguBugun = db.Tbl_Taramalar.Where(a => a.TarihT == bugun).ToList();

            //Tüm Ay
            var ay = DateTime.Now.Month;
            var yıl = DateTime.Now.Year;
            var sqlSorguAy = db.Tbl_Taramalar.Where(a => a.TarihT.Month == ay && a.TarihT.Year == yıl).ToList();

            //Geçen Ay Tarama Hata Sayısı
            var yuzdeler6Ay = new List<double>();
            var yuzdeler9Ay = new List<double>();
            var yuzdelerTopAy = new List<double>();
            var AylikToplam = sqlSorguAy.Select(a => a.IDT).Count();

            for (int i = 0; i <= 10; i++)
            {
                var redSayi9Ay = sqlSorguAy.Count(a => a.DurumT == "RED" && a.SayıT == i && a.TeslimT == "HOL-9");
                var redSayi6Ay = sqlSorguAy.Count(a => a.DurumT == "RED" && a.SayıT == i && a.TeslimT == "HOL-6");
                var redSayiTopAy = sqlSorguAy.Count(a => a.DurumT == "RED" && a.SayıT == i);
                var yuzde6Ay = YuzdeHesap(redSayi6Ay, AylikToplam);
                var yuzde9Ay = YuzdeHesap(redSayi6Ay, AylikToplam);
                var yuzdeTopAy = YuzdeHesap(redSayi6Ay, AylikToplam);
                yuzdeler6Ay.Add(yuzde6Ay);
                yuzdeler9Ay.Add(yuzde9Ay);
                yuzdelerTopAy.Add(yuzdeTopAy);

            }
            ViewBag.AyHataYuzdelerTop = yuzdelerTopAy;
            ViewBag.AyHataYuzdeler6 = yuzdeler6Ay;
            ViewBag.AyHataYuzdeler9 = yuzdeler9Ay;

            //ViewModel
            var viewModel = new TaramaViewModel
            {
                TodayResults = sqlSorguBugun,
                LastWeekResults = sqlSorguDash,
                LastMonthResults = sqlSorguAy,
                LastDayResults = sqlSorguDun,
            };

            return View(viewModel);
        }

        
        public ActionResult Productivity()
        {
            //Dün

            var totalSayıDokumHaftaSQL = from a in db.Tbl_Taramalar.SqlQuery("SET DATEFIRST 5 SELECT * FROM Tbl_Taramalar where DATEPART(week,TarihT) = DATEPART(week,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                         select a;

            double totalSayıDokumHafta = totalSayıDokumHaftaSQL.Select(a => a.IDT).Count();
            double totalGRCHafta = totalSayıDokumHaftaSQL.Where(a => a.DökümT == "GRC").Select(a => a.IDT).Count();
            double totalUHPCHafta = totalSayıDokumHaftaSQL.Where(a => a.DökümT == "UHPC").Select(a => a.IDT).Count();
            double GRCSonucHafta = 0;
            double UHPCSonucHafta = 0;

            if (totalGRCHafta != 0)
            {
                GRCSonucHafta = (totalGRCHafta / totalSayıDokumHafta) * 100;
                GRCSonucHafta = Math.Round(GRCSonucHafta, 0);
                ViewBag.GrcSonucHafta = GRCSonucHafta;

            }
            else
            {
                GRCSonucHafta = 0;
                ViewBag.GrcSonucHafta = GRCSonucHafta;
            }

            if (totalUHPCHafta != 0)
            {
                UHPCSonucHafta = (totalUHPCHafta / totalSayıDokumHafta) * 100;
                UHPCSonucHafta = Math.Round(UHPCSonucHafta, 0);
                ViewBag.UhpcSonucHafta = UHPCSonucHafta;
            }
            else
            {
                UHPCSonucHafta = 0;
                ViewBag.UhpcSonucHafta = UHPCSonucHafta;
            }



            double totalGrcHataHafta = totalSayıDokumHaftaSQL.Where(a => a.DökümT == "GRC" && a.DurumT == "RED").Select(a => a.IDT).Count();
            double totalUhpcHataHafta = totalSayıDokumHaftaSQL.Where(a => a.DökümT == "UHPC" && a.DurumT == "RED").Select(a => a.IDT).Count();
            double GrcHataSonucHafta = 0;
            double UHPCHataSonucHafta = 0;

            if (totalGrcHataHafta != 0)
            {
                GrcHataSonucHafta = (totalGrcHataHafta / totalGRCHafta) * 100;
                GrcHataSonucHafta = Math.Round(GrcHataSonucHafta, 0);
                ViewBag.GrcHataSonucHafta = GrcHataSonucHafta;
            }
            else
            {
                GrcHataSonucHafta = 0;
                ViewBag.GrcHataSonucHafta = GRCSonucHafta;
            }


            if (totalUhpcHataHafta != 0)
            {
                UHPCHataSonucHafta = (totalUhpcHataHafta / totalUHPCHafta) * 100;
                UHPCHataSonucHafta = Math.Round(UHPCHataSonucHafta, 0);
                ViewBag.UhpcHataSonucHafta = UHPCHataSonucHafta;
            }
            else
            {
                UHPCHataSonucHafta = 0;
                ViewBag.UhpcHataSonucHafta = UHPCHataSonucHafta;
            }

            ViewBag.totalGrcSayıHafta = totalGRCHafta;
            ViewBag.totalUhpcSayıHafta = totalUHPCHafta;
            ViewBag.GrcHataSayıHafta = totalGrcHataHafta;
            ViewBag.UhpcHataSayıHafta = totalUhpcHataHafta;

            var dunTotalSayıSQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM TBL_Taramalar where TarihT = DATEADD(day,DATEDIFF(day,1,GETDATE()),0)").ToList()
                                  select a;

            var totalSureCihazDun = dunTotalSayıSQL.Select(a => a.SureT).Sum();
            ViewBag.totalSureCihazDun = totalSureCihazDun;
            var totalSayıDun = dunTotalSayıSQL.Select(a => a.IDT).Count();
            ViewBag.totalSureRaporDun = totalSayıDun;
            ViewBag.m2DunSayı = dunTotalSayıSQL.Select(a => a.AlanT).Sum();
            ViewBag.sureTDunSayı = dunTotalSayıSQL.Select(a => a.SureT).Sum();
            ViewBag.sureRDunSayı = dunTotalSayıSQL.Select(a => a.SureR).Sum();

            //Grad Panel Dün
            var dunSayıSQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where MalzemeT != 'BETON' AND TarihT = dateadd(day,datediff(day,1,GETDATE()),0)").ToList()
                             select a;

            var dun6Sayı = dunSayıSQL.Where(a => a.TeslimT == "HOL-6").Count();
            var dun9Sayı = dunSayıSQL.Where(a => a.TeslimT == "HOL-9").Count();

            //Grafik Dün
            ViewBag.dun6Sayı = dun6Sayı;
            ViewBag.dun9Sayı = dun9Sayı;

            var toplamGradDun = dunSayıSQL.Select(a => a.IDT).Count();


            var dunTOPLAMTOTAL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where TarihT = dateadd(day,datediff(day,1,GETDATE()),0)").ToList()
                                 select a;
            var dunTOPLAMTOTALco = dunTOPLAMTOTAL.Select(a => a.IDT).Count();
            ViewBag.toplamGradDun = dunTOPLAMTOTALco;

            var toplamM2Dun = dunSayıSQL.Select(a => a.AlanT).Sum();
            ViewBag.toplamM2Dun = toplamM2Dun;
            var toplamTSureDun = dunSayıSQL.Select(a => a.SureT).Sum();
            ViewBag.toplamTSureDun = toplamTSureDun;
            var toplamRSureDun = dunSayıSQL.Select(a => a.SureR).Sum();
            ViewBag.toplamRSureDun = toplamRSureDun;

            var betonDunSQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where MalzemeT = 'BETON' AND TarihT = dateadd(day,datediff(day,1,GETDATE()),0)").ToList()
                              select a;
            var betonSayıDun = betonDunSQL.Select(a => a.IDT).Count();
            ViewBag.betonSayıDun = betonSayıDun;
            var betonM2Dun = betonDunSQL.Select(a => a.AlanT).Sum();
            ViewBag.betonM2Dun = betonM2Dun;

            //Kalıp Tipleri
            var TipSQLDun = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where TarihT = dateadd(day,datediff(day,1,GETDATE()),0)").ToList()
                            select a;

            var ULZSayıDun = Convert.ToDouble(TipSQLDun.Where(a => a.YüzeyT == "U-L-Z" && a.MalzemeT != "BETON").Count());
            ViewBag.ulzSayıDun = ULZSayıDun;
            var ULZHataDun = Convert.ToDouble(TipSQLDun.Where(a => a.YüzeyT == "U-L-Z" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.ulzHataDun = ULZSayıDun;

            if (ULZHataDun != 0)
            {
                var ulzSonucDun = ((ULZHataDun / ULZSayıDun) * 100);
                ulzSonucDun = Math.Round(ulzSonucDun, 0);
                ViewBag.ulzSonucDun = ulzSonucDun;
            }
            else
            {
                ViewBag.ulzSonucDun = 0;
            }

            var amorfSayıDun = Convert.ToDouble(TipSQLDun.Where(a => a.YüzeyT == "AMORF" && a.MalzemeT != "BETON").Count());
            ViewBag.amorfSayıDun = amorfSayıDun;
            var amorfHataDun = Convert.ToDouble(TipSQLDun.Where(a => a.YüzeyT == "AMORF" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.amorfHataDun = amorfHataDun;

            if (amorfHataDun != 0)
            {
                var amorfSonucDun = ((amorfHataDun / amorfSayıDun) * 100);
                amorfSonucDun = Math.Round(amorfSonucDun, 0);
                ViewBag.amorfSonucDun = amorfSonucDun;
            }
            else
            {
                ViewBag.amorfSonucDun = 0;
            }

            var duzSayıDun = Convert.ToDouble(TipSQLDun.Where(a => a.YüzeyT == "DÜZ" && a.MalzemeT != "BETON").Count());
            ViewBag.duzSayıDun = duzSayıDun;
            var duzHataDun = Convert.ToDouble(TipSQLDun.Where(a => a.YüzeyT == "DÜZ" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.duzHataDun = duzHataDun;

            if (duzHataDun != 0)
            {
                var duzSonucDun = ((duzHataDun / duzSayıDun) * 100);
                duzSonucDun = Math.Round(duzSonucDun, 0);
                ViewBag.duzSonucDun = duzSonucDun;
            }
            else
            {
                ViewBag.duzSonucDun = 0;
            }

            var tipTotalSayıDun = Convert.ToDouble(TipSQLDun.Where(a => a.MalzemeT != "BETON").Select(a => a.IDT).Count());

            //ULZ- ORAN
            if (ULZSayıDun != 0)
            {
                var ulzOranSonucDun = ((ULZSayıDun / tipTotalSayıDun) * 100);
                ulzOranSonucDun = Math.Round(ulzOranSonucDun, 0);
                ViewBag.ulzOranSonucDun = ulzOranSonucDun;
            }
            else
            {
                ViewBag.ulzOranSonucDun = 0;
            }

            //AMORF ORAN

            if (amorfSayıDun != 0)
            {
                var amorfOranSonuc = ((amorfSayıDun / tipTotalSayıDun) * 100);
                amorfOranSonuc = Math.Round(amorfOranSonuc, 0);
                ViewBag.amorfOranSonucDun = amorfOranSonuc;
            }
            else
            {
                ViewBag.amorfOranSonucDun = 0;
            }


            //DÜZ ORAN

            if (duzSayıDun != 0)
            {
                var duzOranSonucDun = ((duzSayıDun / tipTotalSayıDun) * 100);
                duzOranSonucDun = Math.Round(duzOranSonucDun, 0);
                ViewBag.duzOranSonucDun = duzOranSonucDun;
            }
            else
            {
                ViewBag.duzOranSonucDun = 0;
            }

            //Cihaz Verimlilikleri

            //METRA
            var cihazTotalSureDun = Convert.ToDouble(TipSQLDun.Select(a => a.SureT).Sum());



            var metraSayıDun = TipSQLDun.Where(a => a.CihazT == "METRASCAN").Count();
            ViewBag.metraSayıDun = metraSayıDun;
            var metraSureDun = Convert.ToDouble(TipSQLDun.Where(a => a.CihazT == "METRASCAN").Select(a => a.SureT).Sum());
            ViewBag.metraSureDun = metraSureDun;

            if (metraSureDun != 0)
            {
                var metraSureSonucDun = ((metraSureDun / cihazTotalSureDun) * 100);
                metraSureSonucDun = Math.Round(metraSureSonucDun, 0);
                ViewBag.metraSureSonucDun = metraSureSonucDun;
            }
            else
            {
                ViewBag.metraSureSonucDun = 0;
            }

            //BlackElite

            var eliteSayıDun = TipSQLDun.Where(a => a.CihazT == "BLACK ELİTE").Count();
            ViewBag.eliteSayıDun = eliteSayıDun;
            var eliteSureDun = Convert.ToDouble(TipSQLDun.Where(a => a.CihazT == "BLACK ELİTE").Select(a => a.SureT).Sum());
            ViewBag.eliteSureDun = eliteSureDun;

            if (eliteSureDun != 0)
            {
                var eliteSureSonucDun = ((eliteSureDun / cihazTotalSureDun) * 100);
                eliteSureSonucDun = Math.Round(eliteSureSonucDun, 0);
                ViewBag.eliteSureSonucDun = eliteSureSonucDun;
            }
            else
            {
                ViewBag.eliteSureSonucDun = 0;
            }

            //AICON

            var AiconSayıDun = TipSQLDun.Where(a => a.CihazT == "AICON").Count();
            ViewBag.aiconSayıDun = AiconSayıDun;
            var AiconSureDun = Convert.ToDouble(TipSQLDun.Where(a => a.CihazT == "AICON").Select(a => a.SureT).Sum());
            ViewBag.aiconSureDun = AiconSureDun;

            if (AiconSayıDun != 0)
            {
                var AiconSureSonucDun = ((AiconSureDun / cihazTotalSureDun) * 100);
                AiconSureSonucDun = Math.Round(AiconSureSonucDun, 0);
                ViewBag.aiconSureSonucDun = AiconSureSonucDun;
            }
            else
            {
                ViewBag.aiconSureSonucDun = 0;
            }

            //Tarama Konuları

            //Döküm Öncesi
            var dokumSayıDun = Convert.ToDouble(dunTotalSayıSQL.Where(a => a.KonuT == "DÖKÜM ÖNCESİ").Count());
            ViewBag.dokumSayıDun = dokumSayıDun;
            if (dokumSayıDun != 0)
            {
                var dokumSonucDun = ((dokumSayıDun / totalSayıDun) * 100);
                dokumSonucDun = Math.Round(dokumSonucDun, 0);
                ViewBag.dokumSonucDun = dokumSonucDun;
            }
            else
            {
                ViewBag.dokumSonucDun = 0;
            }

            //Kalıp Toplama
            var toplamaSayıDun = Convert.ToDouble(dunTotalSayıSQL.Where(a => a.KonuT == "KALIP TOPLAMA").Count());
            ViewBag.toplamaSayıDun = toplamaSayıDun;
            if (toplamaSayıDun != 0)
            {
                var toplamaSonucDun = ((toplamaSayıDun / totalSayıDun) * 100);
                toplamaSonucDun = Math.Round(toplamaSonucDun, 0);
                ViewBag.toplamaSonucDun = toplamaSonucDun;
            }
            else
            {
                ViewBag.toplamaSonucDun = 0;
            }

            //DENEME
            var denemeSayıDun = Convert.ToDouble(dunTotalSayıSQL.Where(a => a.KonuT == "DENEME").Count());
            ViewBag.denemeSayıDun = denemeSayıDun;
            if (denemeSayıDun != 0)
            {
                var denemeSonucDun = ((denemeSayıDun / totalSayıDun) * 100);
                denemeSonucDun = Math.Round(denemeSonucDun, 0);
                ViewBag.denemeSonucDun = denemeSonucDun;
            }
            else
            {
                ViewBag.denemeSonucDun = 0;
            }

            //CNC İşleme
            var cncSayıDun = Convert.ToDouble(dunTotalSayıSQL.Where(a => a.KonuT == "CNC İŞLEME").Count());
            ViewBag.cncSayıDun = cncSayıDun;
            if (cncSayıDun != 0)
            {
                var cncSonucDun = ((cncSayıDun / totalSayıDun) * 100);
                cncSonucDun = Math.Round(cncSonucDun, 0);
                ViewBag.cncSonucDun = cncSonucDun;
            }
            else
            {
                ViewBag.cncSonucDun = 0;
            }

            //Döküm Sonrası

            var sonraSayıDun = Convert.ToDouble(dunTotalSayıSQL.Where(a => a.KonuT == "DÖKÜM SONRASI").Count());
            ViewBag.sonraSayıDun = sonraSayıDun;

            if (sonraSayıDun != 0)
            {
                var sonraSonucDun = ((sonraSayıDun / totalSayıDun) * 100);
                sonraSonucDun = Math.Round(sonraSonucDun, 0);
                ViewBag.sonraSonucDun = sonraSonucDun;
            }
            else
            {
                ViewBag.sonraSonucDun = 0;
            }

            //Malzeme Türü

            //STRAFOR
            var straforSayıDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "STRAFOR KALIP").Count());
            var straforHataDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "STRAFOR KALIP" && a.DurumT == "RED").Count());
            ViewBag.straforHataDun = straforHataDun;
            ViewBag.straforSayıDun = straforSayıDun;

            if (straforSayıDun != 0)
            {
                var straforSonucDun = ((straforHataDun / straforSayıDun) * 100);
                straforSonucDun = Math.Round(straforSonucDun, 0);
                ViewBag.straforSonucDun = straforSonucDun;
            }
            else
            {
                ViewBag.straforSonucDun = 0;
            }

            //AHŞAP
            var ahsapSayıDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "AHŞAP KALIP").Count());
            var ahsapHataDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "AHŞAP KALIP" && a.DurumT == "RED").Count());
            ViewBag.ahsapHataDun = ahsapHataDun;
            ViewBag.ahsapSayıDun = ahsapSayıDun;

            if (ahsapSayıDun != 0)
            {
                var ahsapSonucDun = (ahsapHataDun / ahsapSayıDun) * 100;
                ahsapSonucDun = Math.Round(ahsapSonucDun, 0);
                ViewBag.ahsapSonucDun = ahsapSonucDun;
            }
            else
            {
                ViewBag.ahsapSonucDun = 0;
            }

            //POLYESTER

            var polyesterSayıDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "POLYESTER KALIP" || a.MalzemeT == "SAC KALIP" || a.MalzemeT == "KAUÇUK KALIP" || a.MalzemeT == "TADİLAT KALIP").Count());
            var polyesterHataDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "POLYESTER KALIP" || a.MalzemeT == "SAC KALIP" || a.MalzemeT == "KAUÇUK KALIP" || a.MalzemeT == "TADİLAT KALIP" && a.DurumT == "RED").Count());
            ViewBag.polyesterHataDun = polyesterHataDun;
            ViewBag.polyesterSayıDun = polyesterSayıDun;

            if (polyesterSayıDun != 0)
            {
                var polyesterSonucDun = (polyesterHataDun / polyesterSayıDun) * 100;
                polyesterSonucDun = Math.Round(polyesterSonucDun, 0);
                ViewBag.polyesterSonucDun = polyesterSonucDun;
            }
            else
            {
                ViewBag.polyesterSonucDun = 0;
            }

            //ALÇI

            var alciSayıDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "ALÇI KALIP").Count());
            var alciHataDun = Convert.ToDouble(dunSayıSQL.Where(a => a.MalzemeT == "ALÇI KALIP" && a.DurumT == "RED").Count());
            ViewBag.alciSayıDun = alciSayıDun;
            ViewBag.alciHataDun = alciHataDun;

            if (alciSayıDun != 0)
            {
                var alciSonucDun = ((alciHataDun / alciSayıDun) * 100);
                alciSonucDun = Math.Round(alciSonucDun, 0);
                ViewBag.alciSonucDun = alciSonucDun;
            }
            else
            {
                ViewBag.alciSonucDun = 0;
            }


            //Haftalık


            var HaftaTotalSHaftaSQL = from a in db.Tbl_Taramalar.SqlQuery("SET DATEFIRST 5 SELECT * FROM TBL_Taramalar where DATEPART(week,TarihT) = DATEPART(week,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                      select a;

            var totalSureCihazHafta = HaftaTotalSHaftaSQL.Select(a => a.SureT).Sum();
            ViewBag.totalSureCihazHafta = totalSureCihazHafta;
            var totalSayıHafta = HaftaTotalSHaftaSQL.Select(a => a.IDT).Count();
            ViewBag.totalSureRaporHafta = totalSayıHafta;
            ViewBag.m2HaftaSayı = HaftaTotalSHaftaSQL.Select(a => a.AlanT).Sum();
            ViewBag.sureTHaftaSayı = HaftaTotalSHaftaSQL.Select(a => a.SureT).Sum();
            ViewBag.sureRHaftaSayı = HaftaTotalSHaftaSQL.Select(a => a.SureR).Sum();

            //Grad Panel Hafta
            var HaftaSayıSQL = from a in db.Tbl_Taramalar.SqlQuery("SET DATEFIRST 5 SELECT * from Tbl_Taramalar where MalzemeT != 'BETON' AND DATEPART(week,TarihT) = DATEPART(week,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                               select a;

            var Hafta6Sayı = HaftaSayıSQL.Where(a => a.TeslimT == "HOL-6").Count();
            var Hafta9Sayı = HaftaSayıSQL.Where(a => a.TeslimT == "HOL-9").Count();

            //Grafik Hafta
            ViewBag.Hafta6Sayı = Hafta6Sayı;
            ViewBag.Hafta9Sayı = Hafta9Sayı;

            var toplamGradHafta = HaftaSayıSQL.Select(a => a.IDT).Count();


            var HaftaTOPLAMTOTAL = from a in db.Tbl_Taramalar.SqlQuery("SET DATEFIRST 5 SELECT * from Tbl_Taramalar where DATEPART(week,TarihT) = DATEPART(week,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                   select a;
            var HaftaTOPLAMTOTALco = HaftaTOPLAMTOTAL.Select(a => a.IDT).Count();
            ViewBag.toplamGradHafta = HaftaTOPLAMTOTALco;

            var toplamM2Hafta = HaftaSayıSQL.Select(a => a.AlanT).Sum();
            ViewBag.toplamM2Hafta = toplamM2Hafta;
            var toplamTSureHafta = HaftaSayıSQL.Select(a => a.SureT).Sum();
            ViewBag.toplamTSureHafta = toplamTSureHafta;
            var toplamRSureHafta = HaftaSayıSQL.Select(a => a.SureR).Sum();
            ViewBag.toplamRSureHafta = toplamRSureHafta;

            var betonHaftaSQL = from a in db.Tbl_Taramalar.SqlQuery("SET DATEFIRST 5 SELECT * from Tbl_Taramalar where MalzemeT = 'BETON' AND DATEPART(week,TarihT) = DATEPART(week,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                select a;
            var betonSayıHafta = betonHaftaSQL.Select(a => a.IDT).Count();
            ViewBag.betonSayıHafta = betonSayıHafta;
            var betonM2Hafta = betonHaftaSQL.Select(a => a.AlanT).Sum();
            ViewBag.betonM2Hafta = betonM2Hafta;

            //Kalıp Tipleri Hafta
            var TipSQLHafta = from a in db.Tbl_Taramalar.SqlQuery("SET DATEFIRST 5 SELECT * from Tbl_Taramalar where DATEPART(week,TarihT) = DATEPART(week,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                              select a;

            var ULZSayıHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.YüzeyT == "U-L-Z" && a.MalzemeT != "BETON").Count());
            ViewBag.ulzSayıHafta = ULZSayıHafta;
            var ULZHataHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.YüzeyT == "U-L-Z" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.ulzHataHafta = ULZSayıHafta;

            if (ULZHataHafta != 0)
            {
                var ulzSonucHafta = ((ULZHataHafta / ULZSayıHafta) * 100);
                ulzSonucHafta = Math.Round(ulzSonucHafta, 0);
                ViewBag.ulzSonucHafta = ulzSonucHafta;
            }
            else
            {
                ViewBag.ulzSonucHafta = 0;
            }

            var amorfSayıHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.YüzeyT == "AMORF" && a.MalzemeT != "BETON").Count());
            ViewBag.amorfSayıHafta = amorfSayıHafta;
            var amorfHataHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.YüzeyT == "AMORF" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.amorfHataHafta = amorfHataHafta;

            if (amorfHataHafta != 0)
            {
                var amorfSonucHafta = ((amorfHataHafta / amorfSayıHafta) * 100);
                amorfSonucHafta = Math.Round(amorfSonucHafta, 0);
                ViewBag.amorfSonucHafta = amorfSonucHafta;
            }
            else
            {
                ViewBag.amorfSonucHafta = 0;
            }

            var duzSayıHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.YüzeyT == "DÜZ" && a.MalzemeT != "BETON").Count());
            ViewBag.duzSayıHafta = duzSayıHafta;
            var duzHataHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.YüzeyT == "DÜZ" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.duzHataHafta = duzHataHafta;

            if (duzHataHafta != 0)
            {
                var duzSonucHafta = ((duzHataHafta / duzSayıHafta) * 100);
                duzSonucHafta = Math.Round(duzSonucHafta, 0);
                ViewBag.duzSonucHafta = duzSonucHafta;
            }
            else
            {
                ViewBag.duzSonucHafta = 0;
            }

            var tipTotalSayıHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.MalzemeT != "BETON").Select(a => a.IDT).Count());

            //ULZ- ORAN
            if (ULZSayıHafta != 0)
            {
                var ulzOranSonucHafta = ((ULZSayıHafta / tipTotalSayıHafta) * 100);
                ulzOranSonucHafta = Math.Round(ulzOranSonucHafta, 0);
                ViewBag.ulzOranSonucHafta = ulzOranSonucHafta;
            }
            else
            {
                ViewBag.ulzOranSonucHafta = 0;
            }

            //AMORF ORAN

            if (amorfSayıHafta != 0)
            {
                var amorfOranSonuc = ((amorfSayıHafta / tipTotalSayıHafta) * 100);
                amorfOranSonuc = Math.Round(amorfOranSonuc, 0);
                ViewBag.amorfOranSonucHafta = amorfOranSonuc;
            }
            else
            {
                ViewBag.amorfOranSonucHafta = 0;
            }


            //DÜZ ORAN

            if (duzSayıHafta != 0)
            {
                var duzOranSonucHafta = ((duzSayıHafta / tipTotalSayıHafta) * 100);
                duzOranSonucHafta = Math.Round(duzOranSonucHafta, 0);
                ViewBag.duzOranSonucHafta = duzOranSonucHafta;
            }
            else
            {
                ViewBag.duzOranSonucHafta = 0;
            }

            //Döküm Türü

            var totalSayıDokumDunSQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM Tbl_Taramalar where TarihT = dateadd(day,datediff(day,1,GETDATE()),0)").ToList()
                                       select a;

            double totalSayıDokumDun = totalSayıDokumDunSQL.Select(a => a.IDT).Count();
            double totalGRCDun = totalSayıDokumDunSQL.Where(a => a.DökümT == "GRC").Select(a => a.IDT).Count();

            double totalUHPCDun = totalSayıDokumDunSQL.Where(a => a.DökümT == "UHPC").Select(a => a.IDT).Count();


            if (totalGRCDun != 0)
            {
                double GRCSonucDun = (totalGRCDun / totalSayıDokumDun) * 100;
                GRCSonucDun = Math.Round(GRCSonucDun, 0);
                ViewBag.GrcSonucDun = GRCSonucDun;

            }
            else
            {
                double GRCSonucDun = 0;
                ViewBag.GrcSonucDun = GRCSonucDun;
            }

            if (totalUHPCDun != 0)
            {
                double UHPCSonucDun = (totalUHPCDun / totalSayıDokumDun) * 100;
                UHPCSonucDun = Math.Round(UHPCSonucDun, 0);
                ViewBag.UhpcSonucDun = UHPCSonucDun;
            }
            else
            {
                double UHPCSonucDun = 0;
                ViewBag.UhpcSonucDun = UHPCSonucDun;
            }


            double totalGrcHataDun = totalSayıDokumDunSQL.Where(a => a.DökümT == "GRC" && a.DurumT == "RED").Select(a => a.IDT).Count();
            double totalUhpcHataDun = totalSayıDokumDunSQL.Where(a => a.DökümT == "UHPC" && a.DurumT == "RED").Select(a => a.IDT).Count();

            if (totalGrcHataDun != 0)
            {
                double GrcHataSonucDun = (totalGrcHataDun / totalGRCDun) * 100;
                GrcHataSonucDun = Math.Round(GrcHataSonucDun, 2);
                ViewBag.GrcHataSonucDun = GrcHataSonucDun;
            }
            else
            {
                double GrcHataSonucDun = 0;
                ViewBag.GrcHataSonucDun = GrcHataSonucDun;
            }


            if (totalUhpcHataDun != 0)
            {
                double UHPCHataSonucDun = (totalUhpcHataDun / totalUHPCDun) * 100;
                UHPCHataSonucDun = Math.Round(UHPCHataSonucDun, 0);
                ViewBag.UhpcHataSonucDun = UHPCHataSonucDun;

            }
            else
            {
                double UHPCHataSonucDun = 0;
                ViewBag.UhpcHataSonucDun = UHPCHataSonucDun;
            }




            ViewBag.totalGrcSayıDun = totalGRCDun;
            ViewBag.totalUhpcSayıDun = totalUHPCDun;
            ViewBag.GrcHataSayıDun = totalGrcHataDun;
            ViewBag.UhpcHataSayıDun = totalUhpcHataDun;

            //Cihaz Verimlilikleri

            //METRA
            var cihazTotalSureHafta = Convert.ToDouble(TipSQLHafta.Select(a => a.SureT).Sum());



            var metraSayıHafta = TipSQLHafta.Where(a => a.CihazT == "METRASCAN").Count();
            ViewBag.metraSayıHafta = metraSayıHafta;
            var metraSureHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.CihazT == "METRASCAN").Select(a => a.SureT).Sum());
            ViewBag.metraSureHafta = metraSureHafta;

            if (metraSureHafta != 0)
            {
                var metraSureSonucHafta = ((metraSureHafta / cihazTotalSureHafta) * 100);
                metraSureSonucHafta = Math.Round(metraSureSonucHafta, 0);
                ViewBag.metraSureSonucHafta = metraSureSonucHafta;
            }
            else
            {
                ViewBag.metraSureSonucHafta = 0;
            }

            //BlackElite

            var eliteSayıHafta = TipSQLHafta.Where(a => a.CihazT == "BLACK ELİTE").Count();
            ViewBag.eliteSayıHafta = eliteSayıHafta;
            var eliteSureHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.CihazT == "BLACK ELİTE").Select(a => a.SureT).Sum());
            ViewBag.eliteSureHafta = eliteSureHafta;

            if (eliteSureHafta != 0)
            {
                var eliteSureSonucHafta = ((eliteSureHafta / cihazTotalSureHafta) * 100);
                eliteSureSonucHafta = Math.Round(eliteSureSonucHafta, 0);
                ViewBag.eliteSureSonucHafta = eliteSureSonucHafta;
            }
            else
            {
                ViewBag.eliteSureSonucHafta = 0;
            }

            //AICON

            var AiconSayıHafta = TipSQLHafta.Where(a => a.CihazT == "AICON").Count();
            ViewBag.aiconSayıHafta = AiconSayıHafta;
            var AiconSureHafta = Convert.ToDouble(TipSQLHafta.Where(a => a.CihazT == "AICON").Select(a => a.SureT).Sum());
            ViewBag.aiconSureHafta = AiconSureHafta;

            if (AiconSayıHafta != 0)
            {
                var AiconSureSonucHafta = ((AiconSureHafta / cihazTotalSureHafta) * 100);
                AiconSureSonucHafta = Math.Round(AiconSureSonucHafta, 0);
                ViewBag.aiconSureSonucHafta = AiconSureSonucHafta;
            }
            else
            {
                ViewBag.aiconSureSonucHafta = 0;
            }

            //Tarama Konuları

            //Döküm Öncesi
            var dokumSayıHafta = Convert.ToDouble(HaftaTotalSHaftaSQL.Where(a => a.KonuT == "DÖKÜM ÖNCESİ").Count());
            ViewBag.dokumSayıHafta = dokumSayıHafta;
            if (dokumSayıHafta != 0)
            {
                var dokumSonucHafta = ((dokumSayıHafta / totalSayıHafta) * 100);
                dokumSonucHafta = Math.Round(dokumSonucHafta, 0);
                ViewBag.dokumSonucHafta = dokumSonucHafta;
            }
            else
            {
                ViewBag.dokumSonucHafta = 0;
            }

            //Kalıp Toplama
            var toplamaSayıHafta = Convert.ToDouble(HaftaTotalSHaftaSQL.Where(a => a.KonuT == "KALIP TOPLAMA").Count());
            ViewBag.toplamaSayıHafta = toplamaSayıHafta;
            if (toplamaSayıHafta != 0)
            {
                var toplamaSonucHafta = ((toplamaSayıHafta / totalSayıHafta) * 100);
                toplamaSonucHafta = Math.Round(toplamaSonucHafta, 0);
                ViewBag.toplamaSonucHafta = toplamaSonucHafta;
            }
            else
            {
                ViewBag.toplamaSonucHafta = 0;
            }

            //DENEME
            var denemeSayıHafta = Convert.ToDouble(HaftaTotalSHaftaSQL.Where(a => a.KonuT == "DENEME").Count());
            ViewBag.denemeSayıHafta = denemeSayıHafta;
            if (denemeSayıHafta != 0)
            {
                var denemeSonucHafta = ((denemeSayıHafta / totalSayıHafta) * 100);
                denemeSonucHafta = Math.Round(denemeSonucHafta, 0);
                ViewBag.denemeSonucHafta = denemeSonucHafta;
            }
            else
            {
                ViewBag.denemeSonucHafta = 0;
            }

            //CNC İşleme
            var cncSayıHafta = Convert.ToDouble(HaftaTotalSHaftaSQL.Where(a => a.KonuT == "CNC İŞLEME").Count());
            ViewBag.cncSayıHafta = cncSayıHafta;
            if (cncSayıHafta != 0)
            {
                var cncSonucHafta = ((cncSayıHafta / totalSayıHafta) * 100);
                cncSonucHafta = Math.Round(cncSonucHafta, 0);
                ViewBag.cncSonucHafta = cncSonucHafta;
            }
            else
            {
                ViewBag.cncSonucHafta = 0;
            }

            //Döküm Sonrası

            var sonraSayıHafta = Convert.ToDouble(HaftaTotalSHaftaSQL.Where(a => a.KonuT == "DÖKÜM SONRASI").Count());
            ViewBag.sonraSayıHafta = sonraSayıHafta;

            if (sonraSayıHafta != 0)
            {
                var sonraSonucHafta = ((sonraSayıHafta / totalSayıHafta) * 100);
                sonraSonucHafta = Math.Round(sonraSonucHafta, 0);
                ViewBag.sonraSonucHafta = sonraSonucHafta;
            }
            else
            {
                ViewBag.sonraSonucHafta = 0;
            }

            //Malzeme Türü

            //STRAFOR
            var straforSayıHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "STRAFOR KALIP").Count());
            var straforHataHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "STRAFOR KALIP" && a.DurumT == "RED").Count());
            ViewBag.straforSayıHafta = straforSayıHafta;
            ViewBag.straforHataHafta = straforHataHafta;

            if (straforSayıHafta != 0)
            {
                var straforSonucHafta = ((straforHataHafta / straforSayıHafta) * 100);
                straforSonucHafta = Math.Round(straforSonucHafta, 0);
                ViewBag.straforSonucHafta = straforSonucHafta;
            }
            else
            {
                ViewBag.straforSonucHafta = 0;
            }

            //AHŞAP
            var ahsapSayıHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "AHŞAP KALIP").Count());
            var ahsapHataHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "AHŞAP KALIP" && a.DurumT == "RED").Count());
            ViewBag.ahsapSayıHafta = ahsapSayıHafta;
            ViewBag.ahsapHataHafta = ahsapHataHafta;

            if (ahsapSayıHafta != 0)
            {
                var ahsapSonucHafta = (ahsapHataHafta / ahsapSayıHafta) * 100;
                ahsapSonucHafta = Math.Round(ahsapSonucHafta, 0);
                ViewBag.ahsapSonucHafta = ahsapSonucHafta;
            }
            else
            {
                ViewBag.ahsapSonucHafta = 0;
            }

            //POLYESTER

            var polyesterSayıHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "POLYESTER KALIP" || a.MalzemeT == "SAC KALIP" || a.MalzemeT == "KAUÇUK KALIP" || a.MalzemeT == "TADİLAT KALIP").Count());
            var polyesterHataHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "POLYESTER KALIP" || a.MalzemeT == "SAC KALIP" || a.MalzemeT == "KAUÇUK KALIP" ||a.MalzemeT == "TADİLAT KALIP" && a.DurumT == "RED").Count());
            ViewBag.polyesterSayıHafta = polyesterSayıHafta;
            ViewBag.polyesterHataHafta = polyesterHataHafta;

            if (polyesterSayıHafta != 0)
            {
                var polyesterSonucHafta = (polyesterHataHafta / polyesterSayıHafta) * 100;
                polyesterSonucHafta = Math.Round(polyesterSonucHafta, 0);
                ViewBag.polyesterSonucHafta = polyesterSonucHafta;
            }
            else
            {
                ViewBag.polyesterSonucHafta = 0;
            }

            //ALÇI

            var alciSayıHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "ALÇI KALIP").Count());
            var alciHataHafta = Convert.ToDouble(HaftaSayıSQL.Where(a => a.MalzemeT == "ALÇI KALIP" && a.DurumT == "RED").Count());
            ViewBag.alciSayıHafta = alciSayıHafta;
            ViewBag.alciHataHafta = alciHataHafta;

            if (alciSayıHafta != 0)
            {
                var alciSonucHafta = ((alciHataHafta / alciSayıHafta) * 100);
                alciSonucHafta = Math.Round(alciSonucHafta, 0);
                ViewBag.alciSonucHafta = alciSonucHafta;
            }
            else
            {
                ViewBag.alciSonucHafta = 0;
            }

            //Aylık


            var AyTotalSayıSQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM TBL_Taramalar where DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                 select a;

            var totalSureCihazAy = AyTotalSayıSQL.Select(a => a.SureT).Sum();
            ViewBag.totalSureCihazAy = totalSureCihazAy;
            var totalSayıAy = AyTotalSayıSQL.Select(a => a.IDT).Count();
            ViewBag.totalSureRaporAy = totalSayıAy;
            ViewBag.m2AySayı = AyTotalSayıSQL.Select(a => a.AlanT).Sum();
            ViewBag.sureTAySayı = AyTotalSayıSQL.Select(a => a.SureT).Sum();
            ViewBag.sureRAySayı = AyTotalSayıSQL.Select(a => a.SureR).Sum();

            //Grad Panel Ay
            var AySayıSQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where MalzemeT != 'BETON' AND DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                            select a;

            var Ay6Sayı = AySayıSQL.Where(a => a.TeslimT == "HOL-6").Count();
            var Ay9Sayı = AySayıSQL.Where(a => a.TeslimT == "HOL-9").Count();

            //Grafik Ay
            ViewBag.Ay6Sayı = Ay6Sayı;
            ViewBag.Ay9Sayı = Ay9Sayı;

            var toplamGradAy = AySayıSQL.Select(a => a.IDT).Count();


            var AyTOPLAMTOTAL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                select a;
            var AyTOPLAMTOTALco = AyTOPLAMTOTAL.Select(a => a.IDT).Count();
            ViewBag.toplamGradAy = AyTOPLAMTOTALco;

            var toplamM2Ay = AySayıSQL.Select(a => a.AlanT).Sum();
            ViewBag.toplamM2Ay = toplamM2Ay;
            var toplamTSureAy = AySayıSQL.Select(a => a.SureT).Sum();
            ViewBag.toplamTSureAy = toplamTSureAy;
            var toplamRSureAy = AySayıSQL.Select(a => a.SureR).Sum();
            ViewBag.toplamRSureAy = toplamRSureAy;

            var betonAySQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where MalzemeT = 'BETON' AND DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                             select a;
            var betonSayıAy = betonAySQL.Select(a => a.IDT).Count();
            ViewBag.betonSayıAy = betonSayıAy;
            var betonM2Ay = betonAySQL.Select(a => a.AlanT).Sum();
            ViewBag.betonM2Ay = betonM2Ay;

            //Kalıp Tipleri Ay
            var TipSQLAy = from a in db.Tbl_Taramalar.SqlQuery("SELECT * from Tbl_Taramalar where DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                           select a;

            var ULZSayıAy = Convert.ToDouble(TipSQLAy.Where(a => a.YüzeyT == "U-L-Z" && a.MalzemeT != "BETON").Count());
            ViewBag.ulzSayıAy = ULZSayıAy;
            var ULZHataAy = Convert.ToDouble(TipSQLAy.Where(a => a.YüzeyT == "U-L-Z" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.ulzHataAy = ULZHataAy;

            if (ULZHataAy != 0)
            {
                var ulzSonucAy = ((ULZHataAy / ULZSayıAy) * 100);
                ulzSonucAy = Math.Round(ulzSonucAy, 0);
                ViewBag.ulzSonucAy = ulzSonucAy;
            }
            else
            {
                ViewBag.ulzSonucAy = 0;
            }

            var amorfSayıAy = Convert.ToDouble(TipSQLAy.Where(a => a.YüzeyT == "AMORF" && a.MalzemeT != "BETON").Count());
            ViewBag.amorfSayıAy = amorfSayıAy;
            var amorfHataAy = Convert.ToDouble(TipSQLAy.Where(a => a.YüzeyT == "AMORF" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.amorfHataAy = amorfHataAy;

            if (amorfHataAy != 0)
            {
                var amorfSonucAy = ((amorfHataAy / amorfSayıAy) * 100);
                amorfSonucAy = Math.Round(amorfSonucAy, 0);
                ViewBag.amorfSonucAy = amorfSonucAy;
            }
            else
            {
                ViewBag.amorfSonucAy = 0;
            }

            var duzSayıAy = Convert.ToDouble(TipSQLAy.Where(a => a.YüzeyT == "DÜZ" && a.MalzemeT != "BETON").Count());
            ViewBag.duzSayıAy = duzSayıAy;
            var duzHataAy = Convert.ToDouble(TipSQLAy.Where(a => a.YüzeyT == "DÜZ" && a.DurumT == "RED" && a.MalzemeT != "BETON").Count());
            ViewBag.duzHataAy = duzHataAy;

            if (duzHataAy != 0)
            {
                var duzSonucAy = ((duzHataAy / duzSayıAy) * 100);
                duzSonucAy = Math.Round(duzSonucAy, 0);
                ViewBag.duzSonucAy = duzSonucAy;
            }
            else
            {
                ViewBag.duzSonucAy = 0;
            }

            var tipTotalSayıAy = Convert.ToDouble(TipSQLAy.Where(a => a.MalzemeT != "BETON").Select(a => a.IDT).Count());

            //ULZ- ORAN
            if (ULZSayıAy != 0)
            {
                var ulzOranSonucAy = ((ULZSayıAy / tipTotalSayıAy) * 100);
                ulzOranSonucAy = Math.Round(ulzOranSonucAy, 0);
                ViewBag.ulzOranSonucAy = ulzOranSonucAy;
            }
            else
            {
                ViewBag.ulzOranSonucAy = 0;
            }

            //AMORF ORAN

            if (amorfSayıAy != 0)
            {
                var amorfOranSonuc = ((amorfSayıAy / tipTotalSayıAy) * 100);
                amorfOranSonuc = Math.Round(amorfOranSonuc, 0);
                ViewBag.amorfOranSonucAy = amorfOranSonuc;
            }
            else
            {
                ViewBag.amorfOranSonucAy = 0;
            }


            //DÜZ ORAN

            if (duzSayıAy != 0)
            {
                var duzOranSonucAy = ((duzSayıAy / tipTotalSayıAy) * 100);
                duzOranSonucAy = Math.Round(duzOranSonucAy, 0);
                ViewBag.duzOranSonucAy = duzOranSonucAy;
            }
            else
            {
                ViewBag.duzOranSonucAy = 0;
            }

            //Döküm Türü

            var totalSayıDokumAySQL = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM Tbl_Taramalar where DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                      select a;

            double totalSayıDokumAy = totalSayıDokumAySQL.Select(a => a.IDT).Count();
            double totalGRCAy = totalSayıDokumAySQL.Where(a => a.DökümT == "GRC").Select(a => a.IDT).Count();
            double totalUHPCAy = totalSayıDokumAySQL.Where(a => a.DökümT == "UHPC").Select(a => a.IDT).Count();
            double GRCSonucAy = 0;
            double UHPCSonucAy = 0;


            if (totalGRCAy != 0)
            {
                GRCSonucAy = (totalGRCAy / totalSayıDokumAy) * 100;
                GRCSonucAy = Math.Round(GRCSonucAy, 0);
                ViewBag.GrcSonucAy = GRCSonucAy;

            }
            else
            {
                GRCSonucAy = 0;
                ViewBag.GrcSonucAy = GRCSonucAy;
            }

            if (totalUHPCAy != 0)
            {
                UHPCSonucAy = (totalUHPCAy / totalSayıDokumAy) * 100;
                UHPCSonucAy = Math.Round(UHPCSonucAy, 0);
                ViewBag.UhpcSonucAy = UHPCSonucAy;
            }
            else
            {
                UHPCSonucAy = 0;
                ViewBag.UhpcSonucAy = UHPCSonucAy;
            }



            double totalGrcHataAy = totalSayıDokumAySQL.Where(a => a.DökümT == "GRC" && a.DurumT == "RED").Select(a => a.IDT).Count();

            double totalUhpcHataAy = totalSayıDokumAySQL.Where(a => a.DökümT == "UHPC" && a.DurumT == "RED").Select(a => a.IDT).Count();

            double GrcHataSonucAy = 0;
            double UHPCHataSonucAy = 0;

            if (totalGrcHataAy != 0)
            {
                GrcHataSonucAy = (totalGrcHataAy / totalGRCAy) * 100;
                GrcHataSonucAy = Math.Round(GrcHataSonucAy, 0);
                ViewBag.GrcHataSonucAy = GrcHataSonucAy;
            }
            else
            {
                GrcHataSonucAy = 0;
                ViewBag.GrcHataSonucAy = GrcHataSonucAy;
            }


            if (totalUhpcHataAy != 0)
            {
                UHPCHataSonucAy = (totalUhpcHataAy / totalUHPCAy) * 100;
                UHPCHataSonucAy = Math.Round(UHPCHataSonucAy, 0);
                ViewBag.UhpcHataSonucAy = UHPCHataSonucAy;
            }
            else
            {
                UHPCHataSonucAy = 0;
                ViewBag.UhpcHataSonucAy = UHPCHataSonucAy;
            }


            ViewBag.totalGrcSayıAy = totalGRCAy;
            ViewBag.totalUhpcSayıAy = totalUHPCAy;


            ViewBag.GrcHataSayıAy = totalGrcHataAy;
            ViewBag.UhpcHataSayıAy = totalUhpcHataAy;

            //Cihaz Verimlilikleri

            //METRA
            var cihazTotalSureAy = Convert.ToDouble(TipSQLAy.Select(a => a.SureT).Sum());



            var metraSayıAy = TipSQLAy.Where(a => a.CihazT == "METRASCAN").Count();
            ViewBag.metraSayıAy = metraSayıAy;
            var metraSureAy = Convert.ToDouble(TipSQLAy.Where(a => a.CihazT == "METRASCAN").Select(a => a.SureT).Sum());
            ViewBag.metraSureAy = metraSureAy;

            if (metraSureAy != 0)
            {
                var metraSureSonucAy = ((metraSureAy / cihazTotalSureAy) * 100);
                metraSureSonucAy = Math.Round(metraSureSonucAy, 0);
                ViewBag.metraSureSonucAy = metraSureSonucAy;
            }
            else
            {
                ViewBag.metraSureSonucAy = 0;
            }

            //BlackElite

            var eliteSayıAy = TipSQLAy.Where(a => a.CihazT == "BLACK ELİTE").Count();
            ViewBag.eliteSayıAy = eliteSayıAy;
            var eliteSureAy = Convert.ToDouble(TipSQLAy.Where(a => a.CihazT == "BLACK ELİTE").Select(a => a.SureT).Sum());
            ViewBag.eliteSureAy = eliteSureAy;

            if (eliteSureAy != 0)
            {
                var eliteSureSonucAy = ((eliteSureAy / cihazTotalSureAy) * 100);
                eliteSureSonucAy = Math.Round(eliteSureSonucAy, 0);
                ViewBag.eliteSureSonucAy = eliteSureSonucAy;
            }
            else
            {
                ViewBag.eliteSureSonucAy = 0;
            }

            //AICON

            var AiconSayıAy = TipSQLAy.Where(a => a.CihazT == "AICON").Count();
            ViewBag.aiconSayıAy = AiconSayıAy;
            var AiconSureAy = Convert.ToDouble(TipSQLAy.Where(a => a.CihazT == "AICON").Select(a => a.SureT).Sum());
            ViewBag.aiconSureAy = AiconSureAy;

            if (AiconSayıAy != 0)
            {
                var AiconSureSonucAy = ((AiconSureAy / cihazTotalSureAy) * 100);
                AiconSureSonucAy = Math.Round(AiconSureSonucAy, 0);
                ViewBag.aiconSureSonucAy = AiconSureSonucAy;
            }
            else
            {
                ViewBag.aiconSureSonucAy = 0;
            }

            //Tarama Konuları

            //Döküm Öncesi
            var dokumSayıAy = Convert.ToDouble(AyTotalSayıSQL.Where(a => a.KonuT == "DÖKÜM ÖNCESİ").Count());
            ViewBag.dokumSayıAy = dokumSayıAy;
            if (dokumSayıAy != 0)
            {
                var dokumSonucAy = ((dokumSayıAy / totalSayıAy) * 100);
                dokumSonucAy = Math.Round(dokumSonucAy, 0);
                ViewBag.dokumSonucAy = dokumSonucAy;
            }
            else
            {
                ViewBag.dokumSonucAy = 0;
            }

            //Kalıp Toplama
            var toplamaSayıAy = Convert.ToDouble(AyTotalSayıSQL.Where(a => a.KonuT == "KALIP TOPLAMA").Count());
            ViewBag.toplamaSayıAy = toplamaSayıAy;
            if (toplamaSayıAy != 0)
            {
                var toplamaSonucAy = ((toplamaSayıAy / totalSayıAy) * 100);
                toplamaSonucAy = Math.Round(toplamaSonucAy, 0);
                ViewBag.toplamaSonucAy = toplamaSonucAy;
            }
            else
            {
                ViewBag.toplamaSonucAy = 0;
            }

            //DENEME
            var denemeSayıAy = Convert.ToDouble(AyTotalSayıSQL.Where(a => a.KonuT == "DENEME").Count());
            ViewBag.denemeSayıAy = denemeSayıAy;
            if (denemeSayıAy != 0)
            {
                var denemeSonucAy = ((denemeSayıAy / totalSayıAy) * 100);
                denemeSonucAy = Math.Round(denemeSonucAy, 0);
                ViewBag.denemeSonucAy = denemeSonucAy;
            }
            else
            {
                ViewBag.denemeSonucAy = 0;
            }

            //CNC İşleme
            var cncSayıAy = Convert.ToDouble(AyTotalSayıSQL.Where(a => a.KonuT == "CNC İŞLEME").Count());
            ViewBag.cncSayıAy = cncSayıAy;
            if (cncSayıAy != 0)
            {
                var cncSonucAy = ((cncSayıAy / totalSayıAy) * 100);
                cncSonucAy = Math.Round(cncSonucAy, 0);
                ViewBag.cncSonucAy = cncSonucAy;
            }
            else
            {
                ViewBag.cncSonucAy = 0;
            }

            //Döküm Sonrası

            var sonraSayıAy = Convert.ToDouble(AyTotalSayıSQL.Where(a => a.KonuT == "DÖKÜM SONRASI").Count());
            ViewBag.sonraSayıAy = sonraSayıAy;

            if (sonraSayıAy != 0)
            {
                var sonraSonucAy = ((sonraSayıAy / totalSayıAy) * 100);
                sonraSonucAy = Math.Round(sonraSonucAy, 0);
                ViewBag.sonraSonucAy = sonraSonucAy;
            }
            else
            {
                ViewBag.sonraSonucAy = 0;
            }

            //Malzeme Türü

            //STRAFOR
            var straforSayıAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "STRAFOR KALIP").Count());
            var straforHataAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "STRAFOR KALIP" && a.DurumT == "RED").Count());
            ViewBag.straforSayıAy = straforSayıAy;
            ViewBag.straforHataAy = straforHataAy;

            if (straforSayıAy != 0)
            {
                var straforSonucAy = ((straforHataAy / straforSayıAy) * 100);
                straforSonucAy = Math.Round(straforSonucAy, 0);
                ViewBag.straforSonucAy = straforSonucAy;
            }
            else
            {
                ViewBag.straforSonucAy = 0;
            }

            //AHŞAP
            var ahsapSayıAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "AHŞAP KALIP").Count());
            var ahsapHataAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "AHŞAP KALIP" && a.DurumT == "RED").Count());
            ViewBag.ahsapSayıAy = ahsapSayıAy;
            ViewBag.ahsapHataAy = ahsapHataAy;

            if (ahsapSayıAy != 0)
            {
                var ahsapSonucAy = (ahsapHataAy / ahsapSayıAy) * 100;
                ahsapSonucAy = Math.Round(ahsapSonucAy, 0);
                ViewBag.ahsapSonucAy = ahsapSonucAy;
            }
            else
            {
                ViewBag.ahsapSonucAy = 0;
            }

            //POLYESTER

            var polyesterSayıAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "POLYESTER KALIP" || a.MalzemeT == "SAC KALIP" || a.MalzemeT == "KAUÇUK KALIP" || a.MalzemeT == "TADİLAT KALIP").Count());
            var polyesterHataAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "POLYESTER KALIP" || a.MalzemeT == "SAC KALIP" || a.MalzemeT == "KAUÇUK KALIP" || a.MalzemeT == "TADİLAT KALIP" && a.DurumT == "RED").Count());
            ViewBag.polyesterSayıAy = polyesterSayıAy;
            ViewBag.polyesterHataAy = polyesterHataAy;

            if (polyesterSayıAy != 0)
            {
                var polyesterSonucAy = (polyesterHataAy / polyesterSayıAy) * 100;
                polyesterSonucAy = Math.Round(polyesterSonucAy, 0);
                ViewBag.polyesterSonucAy = polyesterSonucAy;
            }
            else
            {
                ViewBag.polyesterSonucAy = 0;
            }

            //ALÇI

            var alciSayıAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "ALÇI KALIP").Count());
            var alciHataAy = Convert.ToDouble(AySayıSQL.Where(a => a.MalzemeT == "ALÇI KALIP" && a.DurumT == "RED").Count());
            ViewBag.alciSayıAy = alciSayıAy;
            ViewBag.alciHataAy = alciHataAy;

            if (alciSayıAy != 0)
            {
                var alciSonucAy = ((alciHataAy / alciSayıAy) * 100);
                alciSonucAy = Math.Round(alciSonucAy, 0);
                ViewBag.alciSonucAy = alciSonucAy;
            }
            else
            {
                ViewBag.alciSonucAy = 0;
            }


            MultipleModelTarama model = new MultipleModelTarama();
            model.TaramaYestReports = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM Tbl_Taramalar where TarihT = DATEADD(day,DATEDIFF(day,0,GETDATE()),0)").ToList()
                                   select a;

            model.TaramaMonthReports = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM Tbl_Taramalar where DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                  select a;

            model.TaramaWeekReports = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM Tbl_Taramalar where DATEPART(m,TarihT) = DATEPART(m,GETDATE()) and datepart(yyyy,TarihT) = datepart(yyyy,GETDATE())").ToList()
                                      select a;


            

            return View();
        }

        [HttpGet]
        
        public ActionResult KalıphanelerS()
        {
            var query = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM TBL_Taramalar where TarihT = Dateadd(day,datediff(day,0,GETDATE()),0)").ToList()
                        select a;
            return View(query);
        }

        [HttpPost]
        
        public ActionResult KalıphanelerS(DateTime bas, DateTime bit)
        {
            //Seçilen Tarihler Arası

            string SQLsec = "SELECT * FROM TBL_Taramalar where TarihT BETWEEN @t1 and @t2";
            SqlParameter t1 = new SqlParameter("@t1", bas);
            SqlParameter t2 = new SqlParameter("@t2", bit);
            object[] param = new object[] { t1, t2 };
            var query = db.Tbl_Taramalar.SqlQuery(SQLsec, param).ToList();

            return View(query);
        }

        [HttpGet]
        
        public ActionResult ProductivityS()
        {
            var query = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM Tbl_Taramalar where TarihT = DATEADD(day,DATEDIFF(day,0,GETDATE()),0)").ToList()
                        select a;

            return View(query);
        }

        [HttpPost]
        public ActionResult ProductivityS(DateTime start, DateTime end)
        {
            string SQLSec = "SELECT * FROM Tbl_Taramalar where TarihT BETWEEN @t1 and @t2";
            SqlParameter t1 = new SqlParameter("@t1", start);
            SqlParameter t2 = new SqlParameter("@t2", end);
            object[] param = new object[] { t1, t2 };
            var query = db.Tbl_Taramalar.SqlQuery(SQLSec, param).ToList();

            return View(query);
        }

    }
}