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


//[assembly: OwinStartup(typeof(PortalFibrobeton.MvcApplication), "Configuration")]
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


            //MqttService mqttService = new MqttService();

            MqttServiceInstance = new MqttService();
        }
    }
}
