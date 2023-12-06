using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using PortalFibrobeton.Models.Entity;


namespace PortalFibrobeton.Controllers
{

    public class UpdateScanController : Controller
    {
        TaramaEntities db = new TaramaEntities();
        PERA_FIBROEntities db1 = new PERA_FIBROEntities();
        // GET: UpdateScan
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult SelectScan()
        {
            var idt = Convert.ToInt32(Session["IDUs"]);
            var roleSQL = from a in db1.KULLANICI.SqlQuery("SELECT * FROM KULLANICI").ToList()
                          select a;

            ViewBag.idt = idt;
            var role = roleSQL.Where(a => a.ID == idt).Select(a => a.YET_M_3D_TARAMA).FirstOrDefault();
            ViewBag.rolee = role;

            if (role == true)
            {
                var query = from a in db.Tbl_Taramalar.SqlQuery("SELECT * FROM TBL_Taramalar where TarihT = Dateadd(day,datediff(day,0,GETDATE()),0)").ToList()
                            select a;
                return View(query);
            }
            else
            {
                //ViewBag.RoleError = "Bu işlemi yapmak için yetkiniz bulunmamaktadır!";
                return RedirectToAction("Dashboard", "HomePage");
            }
            
        }
        
        [HttpPost]
        public ActionResult SelectScan(string PozT, string BarkodT)
        {
            string SQL = "SELECT * FROM Tbl_Taramalar where PozT = @p1 OR BarkodT = @p2";
            SqlParameter pozPar = new SqlParameter("@p1", PozT);
            SqlParameter pozBar = new SqlParameter("@p2", BarkodT);
            object[] param = new object[] { pozPar, pozBar };
            var query = db.Tbl_Taramalar.SqlQuery(SQL, param).ToList();


            return View(query);
        }

        
        public ActionResult ScanInfo(int id)
        {
            var scan = db.Tbl_Taramalar.Find(id);

            return View("ScanInfo", scan);
        }

        
        public ActionResult Update(Tbl_Taramalar p1)
        {
            var scans = db.Tbl_Taramalar.Find(p1.IDT);
            scans.TarihT = p1.TarihT;
            scans.ProjeT = p1.ProjeT;
            scans.BarkodT = p1.BarkodT;
            scans.PozBarkodT = p1.PozBarkodT;
            scans.PozT = p1.PozT;
            scans.MalzemeT = p1.MalzemeT;
            scans.YüzeyT = p1.YüzeyT;
            scans.KonuT = p1.KonuT;
            scans.SureT = p1.SureT;
            scans.SureR = p1.SureR;
            scans.TeslimT = p1.TeslimT;
            scans.SayıT = p1.SayıT;
            scans.DurumT = p1.DurumT;
            scans.HataT = p1.HataT;
            scans.AlanT = p1.AlanT;
            scans.CihazT = p1.CihazT;
            db.SaveChanges();

            return RedirectToAction("SelectScan");
        }

        
        public ActionResult Delete(int id)
        {
            var scan = db.Tbl_Taramalar.Find(id);
            db.Tbl_Taramalar.Remove(scan);
            db.SaveChanges();

            return RedirectToAction("SelectScan");
        }

    }
}