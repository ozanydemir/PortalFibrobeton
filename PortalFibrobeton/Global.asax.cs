using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;


namespace PortalFibrobeton
{
   
    public class MvcApplication : System.Web.HttpApplication
    {
        public static MqttService MqttServiceInstance { get; private set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            MqttServiceInstance = new MqttService();

            ////Hangfire Veritabanı Cache
            //Microsoft.Owin.Host.SystemWeb.OwinBuilder appBuilder = new Microsoft.Owin.Host.SystemWeb.OwinBuilder();
            //Startup startup = new Startup();
            //startup.Configuration(appBuilder.Build());
        }
    }
}
