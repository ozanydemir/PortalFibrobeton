using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using PortalFibrobeton.Models.Entity;


namespace PortalFibrobeton.Models.Entity
{
    //TaramaEntities db  = new TaramaEntities();
    public class MultipleModelTarama
    {
        //public IEnumerable<Tbl_Taramalar> TaramaDunRapor { get; set; } = new List<Tbl_Taramalar>();

        //public IEnumerable<Tbl_Taramalar> TaramaAyRapor { get; set; } = new List<Tbl_Taramalar>();

        public IEnumerable<Tbl_Taramalar> TaramaYestReports { get; set; }

        public IEnumerable<Tbl_Taramalar> TaramaMonthReports { get; set; }

        public IEnumerable<Tbl_Taramalar> TaramaWeekReports { get; set; }



    }
}