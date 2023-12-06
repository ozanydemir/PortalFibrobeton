using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PortalFibrobeton.Models.Entity;

namespace PortalFibrobeton.Controllers.Login
{
    public class LoginController : Controller
    {
        PERA_FIBROEntities db1 = new PERA_FIBROEntities();
        TaramaEntities db = new TaramaEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(KULLANICI p, Tbl_Log p2)
        {
            var users = db1.KULLANICI.FirstOrDefault(x => x.KULLANICI_ADI == p.KULLANICI_ADI && x.SIFRE == p.SIFRE);
            if (users != null)
            {
                FormsAuthentication.SetAuthCookie(users.KULLANICI_ADI, false);
                Session["UserName"] = users.ADI_SOYADI;
                Session["IDUs"] = users.ID;
                //Session["UserYetScan"] = users.YET_M_3D_TARAMA;

                p2.username = users.ADI_SOYADI;
                p2.date = DateTime.Now;
                db.Tbl_Log.Add(p2);
                db.SaveChanges();


                return RedirectToAction("Dashboard", "HomePage");

            }


            else
            {
                TempData["errorLogin"] = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Departmanlar", "Login");
        }

       public ActionResult Departmanlar()
        {
            return View();
        }
    }
}