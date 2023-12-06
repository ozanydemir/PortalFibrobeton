using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Models.Entity;
using System.Data.SqlClient;
using Newtonsoft.Json;
using PortalFibrobeton.Models;
using PortalFibrobeton.Attributelar;
using System.Xml.Linq;
using System.Globalization;

namespace PortalFibrobeton.Controllers
{
    public class ViewResultsController : Controller
    {
        
        // GET: ViewResults
        TaramaEntities db = new TaramaEntities();

        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        
        public ActionResult ResViewDate()
        {
            var query = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM TBL_Taramalar where TarihT = Dateadd(day,datediff(day,0,GETDATE()),0)").ToList()
            select a;
            return View(query);
        }

        [HttpPost]
        
        [MultipleButtonResults(Name = "action", Argument = "date")]
        public ActionResult ResViewDate(DateTime bas, DateTime bit)
        {

            string SQL = "SELECT * From Tbl_Taramalar where TarihT BETWEEN @t1 and @t2";
            SqlParameter t1 = new SqlParameter("@t1", bas);
            SqlParameter t2 = new SqlParameter("@t2", bit);
            object[] param = new object[] { t1, t2 };
            
            var query = db.Tbl_Taramalar.SqlQuery(SQL, param).ToList();
            return View(query);
        }

        [HttpPost]
        
        [MultipleButtonResults(Name = "action", Argument = "barcode")]
        public ActionResult ResViewBarcode(string PozT, string BarkodT)
        {

            string SQL = "SELECT * From Tbl_Taramalar where PozT=@t3 OR BarkodT=@t4";
            SqlParameter t3 = new SqlParameter("@t3", PozT);
            SqlParameter t4 = new SqlParameter("@t4", BarkodT);
            object[] param = new object[] { t3, t4 };
            var query = db.Tbl_Taramalar.SqlQuery(SQL, param).ToList();

            return View(query);
        }

        
        public ActionResult ViewScan(int id)
        {
            var findID = db.Tbl_Taramalar.Find(id);
            if (findID == null)
            {
                return HttpNotFound();
            }

            return View(findID);
        }

        
        public JsonResult GetSelectScan(int id)
        {
            var model = db.Tbl_Taramalar.FirstOrDefault(x => x.IDT == id);
            string value = "";
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);

        }
    }
}