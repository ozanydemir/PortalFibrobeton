using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Hangfire;
using System.Configuration;
using PortalFibrobeton.Models.Class.PeraRaporlar;
using Microsoft.AspNet.SignalR.Hubs;
using Hangfire.Dashboard;

[assembly: OwinStartup(typeof(PortalFibrobeton.Startup))]

namespace PortalFibrobeton
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();


            //Hangfire Denemesi Yapıldı (Başarısız)

            //string hangfireConnectionString = ConfigurationManager.ConnectionStrings["HangfireConnection"].ConnectionString;
            //GlobalConfiguration.Configuration.UseSqlServerStorage(hangfireConnectionString);

            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] {new AllowAllDashboardAuthorizationFilter()}
            //});
            //app.UseHangfireServer();

            //// Her gün saat 05:00'de tüm önbelleği güncelle
            //RecurringJob.AddOrUpdate<DosyaServisi>(
            //    service => service.KlasorCache(),
            //    "0 5 * * *");
        }

        //public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
        //{
        //    public bool Authorize(DashboardContext context)
        //    {
        //        return true;
        //    }
        //}
    }
}
