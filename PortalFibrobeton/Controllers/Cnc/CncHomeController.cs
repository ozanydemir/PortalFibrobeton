using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Models.Entity;

namespace PortalFibrobeton.Controllers.Cnc
{
    public class CncHomeController : Controller
    {
        cncEntities dbCnc = new cncEntities();

        // GET: CncHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePageCnc()
        {

            //var query = from a in dbCnc.CNC_1.SqlQuery(" SELECT * from CNC_1 WHERE tarih >= DATEADD(day,-7, GETDATE())").ToList()
            //            select a;


            var query = from a in dbCnc.tuketim_degerleri.SqlQuery("SELECT * FROM tuketim_degerleri order by ID DESC").ToList()
                        select a;

            return View(query);
        }

        public ActionResult Deneme()
        {
            return View();
        }
    }
}