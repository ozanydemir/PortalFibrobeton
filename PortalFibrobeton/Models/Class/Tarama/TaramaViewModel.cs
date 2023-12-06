using PortalFibrobeton.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.Tarama
{
    public class TaramaViewModel
    {
        public List<LastTenActivityModelItem> LastTenActivity { get; set; }
        public List<Tbl_Taramalar> LastWeekResults { get; set; }
        public List<Tbl_Taramalar> TodayResults { get; set; }
        public List<Tbl_Taramalar> LastMonthResults { get; set; }
        public List<Tbl_Taramalar> LastDayResults { get; set; }
    }
}