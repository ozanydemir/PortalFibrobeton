using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace PortalFibrobeton.Controllers.Cnc
{
    public class CncProductivityController : Controller
    {
        cncEntities dbCnc = new cncEntities();

        // GET: CncProductivity
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CncReports()
        {

            MultipleModelCnc model = new MultipleModelCnc();

            //Gün Sayısı
            ViewBag.farkgun = 1;
            ViewBag.diff = 86400000;

            //Mevcut Ayın Gün Sayısı
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var days = DateTime.DaysInMonth(year, month);
            ViewBag.DaysCountInMonth = days;

            //Mevcut Gün ve Ms
            var currentday = DateTime.Now.Day;
            ViewBag.currentday = currentday;
            var currentdayToMs = TimeSpan.FromDays(currentday).TotalMilliseconds;
            ViewBag.currentDayMs = currentdayToMs;

            //Gün Sayısını Millisaniyeye Çevirme
            var dayToMs = TimeSpan.FromDays(days).TotalMilliseconds;
            ViewBag.DaysToMsInMonth = dayToMs;

            string lastdate = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
            ViewBag.lastdate = lastdate;

            string last2date = DateTime.Now.AddMonths(-2).ToString("MM-yyyy");
            ViewBag.lastdate2 = last2date;

            string last3date = DateTime.Now.AddMonths(-3).ToString("MM-yyyy");
            ViewBag.lastdate3 = last3date;

            string last4date = DateTime.Now.AddMonths(-4).ToString("MM-yyyy");
            ViewBag.lastdate4 = last4date;

            string last5date = DateTime.Now.AddMonths(-5).ToString("MM-yyyy");
            ViewBag.lastdate5 = last5date;

            string last6date = DateTime.Now.AddMonths(-6).ToString("MM-yyyy");
            ViewBag.last6date = last6date;

            List<string> last6dates = new List<string>();
            last6dates.Add(lastdate);
            last6dates.Add(last2date);
            last6dates.Add(last3date);
            last6dates.Add(last4date);
            last6dates.Add(last5date);
            last6dates.Add(last6date);
            last6dates.Reverse();
            model.CncMonthLast6 = last6dates;
        

            model.CncYestReports = from a in dbCnc.CNC_1.SqlQuery("SELECT * FROM CNC_1 where cast(tarih as Date) = cast(getdate() as Date)").ToList()
                                   select a;

            model.CncWeekReports = from a in dbCnc.CNC_1.SqlQuery("SELECT * FROM CNC_1 where DATEPART(week,tarih) = DATEPART(week,GETDATE()) AND DATEPART(yyyy,tarih) = datepart(yyyy,GETDATE())").ToList()
                                   select a;

            model.CncMonthReport = from a in dbCnc.CNC_1.SqlQuery("SELECT * FROM CNC_1 where DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE())").ToList()
                                   select a;

            model.CNCYestWattReport = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri where tarih >= dateadd(day,datediff(day,1,GETDATE()),0) and tarih < dateadd(day,datediff(day,0,GETDATE()),0) ORDER BY ID").ToList()
                                      select a;

            model.CNCWeekWattReport = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri where tarih >= DATEADD(day,-7, GETDATE()) ORDER BY ID").ToList()
                                      select a;


            //Aylık Değerler
            model.CNCMonthWattMN01 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'MN01' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;
            model.CNCMonthWattMN02 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'MN02' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;

            model.CNCMonthWattCMA03 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'CMA03' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;
            model.CNCMonthWattCMA04 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'CMA04' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;
            model.CNCMonthWattCMA05 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'CMA05' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;
            model.CNCMonthWattCMA06 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'CMA06' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;
            model.CNCMonthWattCMA07 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'CMA07' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                       select a;

            model.CNCMonthWattCMA08 = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where makine_adi = 'CMA08' AND DATEPART(m,tarih) = DATEPART(m,GETDATE()) AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)").ToList()
                                      select a;



            //Geçen Ayın Verileri
            var lastmonthCalisma = from a in dbCnc.CNC_1.SqlQuery("SELECT * FROM CNC_1 WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -1, getdate()))")
                                   select a;


            var lastMonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -1, getdate()))")
                                   select a;

            var last2MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -2, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -2, getdate()))")
                                   select a;

            var last3MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -3, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -3, getdate()))")
                                   select a;

            var last4MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -4, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -4, getdate()))")
                                   select a;

            var last5MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -5, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -5, getdate()))")
                                   select a;

            var last6MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -6, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -6, getdate()))")
                                   select a;

            //MN01
            var MN01LastMCalisma = lastmonthCalisma.Where(a => a.makine_adi == "MN01").Select(a => a.calisma_saati).Sum();
            ViewBag.MN01LastMCalisma = MN01LastMCalisma;
            var MN01LastMTuketimW = lastMonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01LastMTuketimKW = MN01LastMTuketimW / 1000;
            ViewBag.MN01LastMkW = MN01LastMTuketimKW;

            //Son 6 Ay'ın Verileri - MN01
            var MN01Last2T = last2MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last3T = last3MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last4T = last4MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last5T = last5MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last6T = last6MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            List<double> CNCMonthLast6WattMN01 = new List<double>();
            CNCMonthLast6WattMN01.Add((double)MN01LastMTuketimW);
            CNCMonthLast6WattMN01.Add((double)MN01Last2T);
            CNCMonthLast6WattMN01.Add((double)MN01Last3T);
            CNCMonthLast6WattMN01.Add((double)MN01Last4T);
            CNCMonthLast6WattMN01.Add((double)MN01Last5T);
            CNCMonthLast6WattMN01.Add((double)MN01Last6T);
            CNCMonthLast6WattMN01.Reverse();
            model.CNCMonthLast6WattMN01 = CNCMonthLast6WattMN01.ToList();


            //MN02
            var MN02LastM = lastmonthCalisma.Where(a => a.makine_adi == "MN02").Select(a => a.calisma_saati).Sum();
            ViewBag.MN02LastM = MN02LastM;

            var MN02LastMTuketimW = lastMonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02LastMTuketimKW = MN02LastMTuketimW / 1000;
            ViewBag.MN02LastMkW = MN02LastMTuketimKW;

            //Son 6 Ay'ın Verileri
            var MN02Last2T = last2MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last3T = last3MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last4T = last4MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last5T = last5MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last6T = last6MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattMN02 = new List<double>();
            CNCMonthLast6WattMN02.Add((double)MN02LastMTuketimW);
            CNCMonthLast6WattMN02.Add((double)MN02Last2T);
            CNCMonthLast6WattMN02.Add((double)MN02Last3T);
            CNCMonthLast6WattMN02.Add((double)MN02Last4T);
            CNCMonthLast6WattMN02.Add((double)MN02Last5T);
            CNCMonthLast6WattMN02.Add((double)MN02Last6T);
            CNCMonthLast6WattMN02.Reverse();
            model.CNCMonthLast6WattMN02 = CNCMonthLast6WattMN02.ToList();


            return View(model);
        }


        [HttpPost]
        public ActionResult CncReports(DateTime bas, DateTime bit)
        {
            MultipleModelCnc model = new MultipleModelCnc();
            string SQL = "SELECT * FROM CNC_1 where Tarih BETWEEN @p1 and @p2";
            SqlParameter t1 = new SqlParameter("@p1", bas);
            SqlParameter t2 = new SqlParameter("@p2", bit);
            object[] param = new object[] { t1, t2 };

            var query = dbCnc.CNC_1.SqlQuery(SQL, param).ToList();


            return View(model);

        }

        [HttpGet]
        public ActionResult CncReportsD()
        {
            MultipleModelCnc model = new MultipleModelCnc();


            //Geçen 6 Ayın Verileri
            var lastmonthCalisma = from a in dbCnc.CNC_1.SqlQuery("SELECT * FROM CNC_1 WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -1, getdate()))")
                                   select a;
            var lastMonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -1, getdate()))")
                                   select a;
            var last2MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -2, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -2, getdate()))")
                                    select a;
            var last3MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -3, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -3, getdate()))")
                                    select a;
            var last4MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -4, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -4, getdate()))")
                                    select a;
            var last5MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -5, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -5, getdate()))")
                                    select a;
            var last6MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -6, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -6, getdate()))")
                                    select a;


            //Son 6 Ay'ın Verileri - MN01
            double MN01LastTMN01 = (double)lastMonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last2TMN01 = last2MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last3TMN01 = last3MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last4TMN01 = last4MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last5TMN01 = last5MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last6TMN01 = last6MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattMN01 = new List<double>();
            CNCMonthLast6WattMN01.Add(MN01LastTMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last2TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last3TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last4TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last5TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last6TMN01);
            CNCMonthLast6WattMN01.Reverse();
            model.CNCMonthLast6WattMN01 = CNCMonthLast6WattMN01.ToList();



            //Son 6 Ay'ın Verileri - MN02
            double MN02LastTMN02 = (double)lastMonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last2TMN02 = last2MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last3TMN02 = last3MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last4TMN02 = last4MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last5TMN02 = last5MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last6TMN02 = last6MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattMN02 = new List<double>();
            CNCMonthLast6WattMN02.Add(MN02LastTMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last2TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last3TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last4TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last5TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last6TMN02);
            CNCMonthLast6WattMN02.Reverse();
            model.CNCMonthLast6WattMN02 = CNCMonthLast6WattMN02.ToList();


            //Son 6 Ay'ın Verileri - CMA03
            double CMA03LastTCMA03 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last2TCMA03 = last2MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last3TCMA03 = last3MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last4TCMA03 = last4MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last5TCMA03 = last5MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last6TCMA03 = last6MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA03 = new List<double>();
            CNCMonthLast6WattCMA03.Add(CMA03LastTCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last2TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last3TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last4TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last5TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last6TCMA03);
            CNCMonthLast6WattCMA03.Reverse();
            model.CNCMonthLast6WattCMA03 = CNCMonthLast6WattCMA03.ToList();



            //Son 6 Ay'ın Verileri - CMA04
            double CMA04LastTCMA04 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last2TCMA04 = last2MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last3TCMA04 = last3MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last4TCMA04 = last4MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last5TCMA04 = last5MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last6TCMA04 = last6MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA04 = new List<double>();
            CNCMonthLast6WattCMA04.Add(CMA04LastTCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last2TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last3TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last4TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last5TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last6TCMA04);
            CNCMonthLast6WattCMA04.Reverse();
            model.CNCMonthLast6WattCMA04 = CNCMonthLast6WattCMA04.ToList();



            //Son 6 Ay'ın Verileri - CMA05
            double CMA05LastTCMA05 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last2TCMA05 = last2MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last3TCMA05 = last3MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last4TCMA05 = last4MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last5TCMA05 = last5MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last6TCMA05 = last6MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA05 = new List<double>();
            CNCMonthLast6WattCMA05.Add(CMA05LastTCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last2TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last3TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last4TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last5TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last6TCMA05);
            CNCMonthLast6WattCMA05.Reverse();
            model.CNCMonthLast6WattCMA05 = CNCMonthLast6WattCMA05.ToList();


            //Son 6 Ay'ın Verileri - CMA06
            double CMA06LastTCMA06 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last2TCMA06 = last2MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last3TCMA06 = last3MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last4TCMA06 = last4MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last5TCMA06 = last5MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last6TCMA06 = last6MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA06 = new List<double>();
            CNCMonthLast6WattCMA06.Add(CMA06LastTCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last2TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last3TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last4TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last5TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last6TCMA06);
            CNCMonthLast6WattCMA06.Reverse();
            model.CNCMonthLast6WattCMA06 = CNCMonthLast6WattCMA06.ToList();



            //Son 6 Ay'ın Verileri - CMA07
            double CMA07LastTCMA07 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last2TCMA07 = last2MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last3TCMA07 = last3MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last4TCMA07 = last4MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last5TCMA07 = last5MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last6TCMA07 = last6MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA07 = new List<double>();
            CNCMonthLast6WattCMA07.Add(CMA07LastTCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last2TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last3TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last4TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last5TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last6TCMA07);
            CNCMonthLast6WattCMA07.Reverse();
            model.CNCMonthLast6WattCMA07 = CNCMonthLast6WattCMA07.ToList();


            //Son 6 Ay Tarih
            string lastdate = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
            string last2date = DateTime.Now.AddMonths(-2).ToString("MM-yyyy");
            string last3date = DateTime.Now.AddMonths(-3).ToString("MM-yyyy");
            string last4date = DateTime.Now.AddMonths(-4).ToString("MM-yyyy");
            string last5date = DateTime.Now.AddMonths(-5).ToString("MM-yyyy");
            string last6date = DateTime.Now.AddMonths(-6).ToString("MM-yyyy");
            List<string> last6dates = new List<string>();
            last6dates.Add(lastdate);
            last6dates.Add(last2date);
            last6dates.Add(last3date);
            last6dates.Add(last4date);
            last6dates.Add(last5date);
            last6dates.Add(last6date);
            last6dates.Reverse();
            model.CncMonthLast6 = last6dates;



            //Null Değer İçin Yarının Verileri
            model.SecilenTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                   select a;
            model.SecilenSaatler = from a in dbCnc.CNC_1.SqlQuery("select * from CNC_1 where convert(varchar,CNC_1.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                   select a;
            model.CNCSecilenWattMN01 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                       select a;
            model.CNCSecilenWattMN02 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                       select a;
            model.CNCSecilenWattCMA03 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                        select a;
            model.CNCSecilenWattCMA04 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                        select a;
            model.CNCSecilenWattCMA05 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                        select a;
            model.CNCSecilenWattCMA06 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                        select a;
            model.CNCSecilenWattCMA07 = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                        select a;
            model.CncSecilenXSaat = from a in dbCnc.CNC_1.SqlQuery("select * from CNC_1 where convert(varchar,CNC_1.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                    select a;
            model.CNCSecilenXTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("select * from tuketim_degerleri where convert(varchar,tuketim_degerleri.[tarih],101) = convert(varchar, GETDATE() +1, 101)").ToList()
                                       select a;

            
            return View(model);
        }

        [HttpPost]
        public ActionResult CncReportsD(DateTime bas, DateTime bit)
        {

            //DateTimePicker Sorguları - Çalışma Saatleri İçin
            var bit2 = bit.AddDays(2);
            var bas2 = bas.AddDays(1);

            //DateTimePicker - Watt Değeri İçin
            var bitWatt = bit.AddDays(1);

            //DateTimePicker Farkı
            TimeSpan diff = (bas2 - bit2);
            ViewBag.farkgun = (diff.TotalDays) * -1;
            ViewBag.diff = diff.TotalMilliseconds * -1;
            List<DateTime> allDates = new List<DateTime>();
            for(DateTime date = bas; date <= bit; date=date.AddDays(1))
            {
                allDates.Add(date);
            }
            ViewBag.allDates = allDates;


            MultipleModelCnc model = new MultipleModelCnc();
            string SQL = "SELECT * FROM CNC_1 where Tarih BETWEEN @p1 and @p2";
            string SQLTuketim = "SELECT * FROM tuketim_degerleri where tarih BETWEEN @p3 and @p4";
            SqlParameter t1 = new SqlParameter("@p1", bas2);
            SqlParameter t2 = new SqlParameter("@p2", bit2);
            object[] param = new object[] { t1, t2 };
            var querysaat = dbCnc.CNC_1.SqlQuery(SQL, param).ToList();
            model.SecilenSaatler = querysaat;

            SqlParameter t3 = new SqlParameter("@p3", bas);
            SqlParameter t4 = new SqlParameter("@p4", bitWatt);
            object[] param2 = new object[] { t3, t4 };
            var querytuketim = dbCnc.tuketim_degerleri.SqlQuery(SQLTuketim, param2).ToList();
            model.SecilenTuketim = querytuketim;



            //Geçen x Tarihine Göre Veriler

            string gecenXsaat = "SELECT * FROM CNC_1 where tarih BETWEEN @x and @x2";
            DateTime xdate = (bas2 - (-diff));
            SqlParameter x = new SqlParameter("@x", xdate);
            SqlParameter x2 = new SqlParameter("@x2", bas2);
            object[] xpar = new object[] { x, x2 };
            model.CncSecilenXSaat = dbCnc.CNC_1.SqlQuery(gecenXsaat, xpar).ToList();

            string gecenXtuketim = "SELECT * FROM tuketim_degerleri where tarih BETWEEN @y and @y2";
            SqlParameter y = new SqlParameter("@y", xdate);
            SqlParameter y2 = new SqlParameter("@y2", bas2);
            object[] ypar = new object[] { y, y2 };
            model.CNCSecilenXTuketim = dbCnc.tuketim_degerleri.SqlQuery(gecenXtuketim,ypar).ToList();


            //Seçilen Tarihler İçin Watt Toplam Sorgusu

            //MN01
            string MN01 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @m1 and @m2 and makine_adi = 'MN01' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";                         
            SqlParameter m1 = new SqlParameter("@m1", bas);
            SqlParameter m2 = new SqlParameter("@m2", bitWatt);
            object[] mn01param = new object[] { m1, m2 };
            model.CNCSecilenWattMN01 = dbCnc.tuketim_degerleri.SqlQuery(MN01, mn01param).ToList();         

            //MN02
            string MN02 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @m3 and @m4 and makine_adi = 'MN02' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";
            SqlParameter m3 = new SqlParameter("@m3", bas);
            SqlParameter m4 = new SqlParameter("@m4", bitWatt);
            object[] mn02param = new object[] { m3, m4 };
            model.CNCSecilenWattMN02 = dbCnc.tuketim_degerleri.SqlQuery(MN02, mn02param).ToList();

            //CMA03
            string CMA03 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @cm1 and @cm2 and makine_adi = 'CMA03' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";
            SqlParameter cm3 = new SqlParameter("@cm1", bas);
            SqlParameter cm31 = new SqlParameter("@cm2", bitWatt);
            object[] cma03param = new object[] { cm3, cm31 };
            model.CNCSecilenWattCMA03 = dbCnc.tuketim_degerleri.SqlQuery(CMA03, cma03param).ToList();

            //CMA04
            string CMA04 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @cm3 and @cm4 and makine_adi = 'CMA04' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";
            SqlParameter cm4 = new SqlParameter("@cm3", bas);
            SqlParameter cm41 = new SqlParameter("@cm4", bitWatt);
            object[] cma04param = new object[] { cm4, cm41 };
            model.CNCSecilenWattCMA04 = dbCnc.tuketim_degerleri.SqlQuery(CMA04, cma04param).ToList();

            //CMA05
            string CMA05 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @cm5 and @cm6 and makine_adi = 'CMA05' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";
            SqlParameter cm5 = new SqlParameter("@cm5", bas);
            SqlParameter cm51 = new SqlParameter("@cm6", bitWatt);
            object[] cma05param = new object[] { cm5, cm51 };
            model.CNCSecilenWattCMA05 = dbCnc.tuketim_degerleri.SqlQuery(CMA05, cma05param).ToList();

            //CMA06
            string CMA06 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @cm6 and @cm7 and makine_adi = 'CMA06' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";
            SqlParameter cm6 = new SqlParameter("@cm6", bas);
            SqlParameter cm61 = new SqlParameter("@cm7", bitWatt);
            object[] cma06param = new object[] { cm6, cm61 };
            model.CNCSecilenWattCMA06 = dbCnc.tuketim_degerleri.SqlQuery(CMA06, cma06param).ToList();

            //CMA07
            string CMA07 = "SELECT MAX(ID) AS ID,MAX(makine_adi) AS makine_adi, CAST(tarih AS DATE) as Tarih, SUM(tuketim) as Tuketim FROM tuketim_degerleri where tarih between @cm8 and @cm9 and makine_adi = 'CMA07' AND DATEPART(yyyy,tarih) = DATEPART(yyyy,GETDATE()) GROUP BY CAST(tarih AS DATE)";
            SqlParameter cm7 = new SqlParameter("@cm8", bas);
            SqlParameter cm71 = new SqlParameter("@cm9", bitWatt);
            object[] cma07param = new object[] { cm7, cm71 };
            model.CNCSecilenWattCMA07 = dbCnc.tuketim_degerleri.SqlQuery(CMA07, cma07param).ToList();


            //Son 6 Ay SQL Veri
            var lastmonthCalisma = from a in dbCnc.CNC_1.SqlQuery("SELECT * FROM CNC_1 WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -1, getdate()))")
                                   select a;
            var lastMonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -1, getdate()))")
                                   select a;
            var last2MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -2, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -2, getdate()))")
                                    select a;
            var last3MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -3, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -3, getdate()))")
                                    select a;
            var last4MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -4, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -4, getdate()))")
                                    select a;
            var last5MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -5, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -5, getdate()))")
                                    select a;
            var last6MonthTuketim = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri WHERE DATEPART(m, tarih) = DATEPART(m, DATEADD(m, -6, getdate())) AND DATEPART(yyyy, tarih) = DATEPART(yyyy, DATEADD(m, -6, getdate()))")
                                    select a;


            //Son 6 Ay Tarih
            string lastdate = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
            string last2date = DateTime.Now.AddMonths(-2).ToString("MM-yyyy");
            string last3date = DateTime.Now.AddMonths(-3).ToString("MM-yyyy");
            string last4date = DateTime.Now.AddMonths(-4).ToString("MM-yyyy");
            string last5date = DateTime.Now.AddMonths(-5).ToString("MM-yyyy");
            string last6date = DateTime.Now.AddMonths(-6).ToString("MM-yyyy");
            List<string> last6dates = new List<string>();
            last6dates.Add(lastdate);
            last6dates.Add(last2date);
            last6dates.Add(last3date);
            last6dates.Add(last4date);
            last6dates.Add(last5date);
            last6dates.Add(last6date);
            last6dates.Reverse();
            model.CncMonthLast6 = last6dates;


            //Son 6 Ay'ın Verileri - MN01
            double MN01LastTMN01 = (double)lastMonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last2TMN01 = last2MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last3TMN01 = last3MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last4TMN01 = last4MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last5TMN01 = last5MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();
            var MN01Last6TMN01 = last6MonthTuketim.Where(a => a.makine_adi == "MN01").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattMN01 = new List<double>();
            CNCMonthLast6WattMN01.Add(MN01LastTMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last2TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last3TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last4TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last5TMN01);
            CNCMonthLast6WattMN01.Add((double)MN01Last6TMN01);
            CNCMonthLast6WattMN01.Reverse();
            model.CNCMonthLast6WattMN01 = CNCMonthLast6WattMN01.ToList();

            //Son 6 Ay'ın Verileri - MN02
            double MN02LastTMN02 = (double)lastMonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last2TMN02 = last2MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last3TMN02 = last3MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last4TMN02 = last4MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last5TMN02 = last5MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();
            var MN02Last6TMN02 = last6MonthTuketim.Where(a => a.makine_adi == "MN02").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattMN02 = new List<double>();
            CNCMonthLast6WattMN02.Add(MN02LastTMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last2TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last3TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last4TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last5TMN02);
            CNCMonthLast6WattMN02.Add((double)MN02Last6TMN02);
            CNCMonthLast6WattMN02.Reverse();
            model.CNCMonthLast6WattMN02 = CNCMonthLast6WattMN02.ToList();

            //Son 6 Ay'ın Verileri - CMA03
            double CMA03LastTCMA03 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last2TCMA03 = last2MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last3TCMA03 = last3MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last4TCMA03 = last4MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last5TCMA03 = last5MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();
            var CMA03Last6TCMA03 = last6MonthTuketim.Where(a => a.makine_adi == "CMA03").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA03 = new List<double>();
            CNCMonthLast6WattCMA03.Add(CMA03LastTCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last2TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last3TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last4TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last5TCMA03);
            CNCMonthLast6WattCMA03.Add((double)CMA03Last6TCMA03);
            CNCMonthLast6WattCMA03.Reverse();
            model.CNCMonthLast6WattCMA03 = CNCMonthLast6WattCMA03.ToList();



            //Son 6 Ay'ın Verileri - CMA04
            double CMA04LastTCMA04 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last2TCMA04 = last2MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last3TCMA04 = last3MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last4TCMA04 = last4MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last5TCMA04 = last5MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();
            var CMA04Last6TCMA04 = last6MonthTuketim.Where(a => a.makine_adi == "CMA04").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA04 = new List<double>();
            CNCMonthLast6WattCMA04.Add(CMA04LastTCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last2TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last3TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last4TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last5TCMA04);
            CNCMonthLast6WattCMA04.Add((double)CMA04Last6TCMA04);
            CNCMonthLast6WattCMA04.Reverse();
            model.CNCMonthLast6WattCMA04 = CNCMonthLast6WattCMA04.ToList();



            //Son 6 Ay'ın Verileri - CMA05
            double CMA05LastTCMA05 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last2TCMA05 = last2MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last3TCMA05 = last3MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last4TCMA05 = last4MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last5TCMA05 = last5MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();
            var CMA05Last6TCMA05 = last6MonthTuketim.Where(a => a.makine_adi == "CMA05").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA05 = new List<double>();
            CNCMonthLast6WattCMA05.Add(CMA05LastTCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last2TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last3TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last4TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last5TCMA05);
            CNCMonthLast6WattCMA05.Add((double)CMA05Last6TCMA05);
            CNCMonthLast6WattCMA05.Reverse();
            model.CNCMonthLast6WattCMA05 = CNCMonthLast6WattCMA05.ToList();


            //Son 6 Ay'ın Verileri - CMA06
            double CMA06LastTCMA06 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last2TCMA06 = last2MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last3TCMA06 = last3MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last4TCMA06 = last4MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last5TCMA06 = last5MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();
            var CMA06Last6TCMA06 = last6MonthTuketim.Where(a => a.makine_adi == "CMA06").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA06 = new List<double>();
            CNCMonthLast6WattCMA06.Add(CMA06LastTCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last2TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last3TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last4TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last5TCMA06);
            CNCMonthLast6WattCMA06.Add((double)CMA06Last6TCMA06);
            CNCMonthLast6WattCMA06.Reverse();
            model.CNCMonthLast6WattCMA06 = CNCMonthLast6WattCMA06.ToList();



            //Son 6 Ay'ın Verileri - CMA07
            double CMA07LastTCMA07 = (double)lastMonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last2TCMA07 = last2MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last3TCMA07 = last3MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last4TCMA07 = last4MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last5TCMA07 = last5MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();
            var CMA07Last6TCMA07 = last6MonthTuketim.Where(a => a.makine_adi == "CMA07").Select(a => a.tuketim).Sum();

            List<double> CNCMonthLast6WattCMA07 = new List<double>();
            CNCMonthLast6WattCMA07.Add(CMA07LastTCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last2TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last3TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last4TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last5TCMA07);
            CNCMonthLast6WattCMA07.Add((double)CMA07Last6TCMA07);
            CNCMonthLast6WattCMA07.Reverse();
            model.CNCMonthLast6WattCMA07 = CNCMonthLast6WattCMA07.ToList();


            return View(model);
        }
    }
}