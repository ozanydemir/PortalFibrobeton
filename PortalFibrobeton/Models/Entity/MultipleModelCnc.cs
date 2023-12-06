using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Entity
{
    public class MultipleModelCnc
    {
        public IEnumerable<CNC_1> CncYestReports { get; set; }

        public IEnumerable<CNC_1> CncWeekReports { get; set; }

        public IEnumerable<CNC_1> CncMonthReport { get; set; }

        public IEnumerable<tuketim_degerleri> CNCYestWattReport { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattMN01 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattMN02 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattCMA03 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattCMA04 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattCMA05 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattCMA06 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattCMA07 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCMonthWattCMA08 { get; set; }

        public IEnumerable<tuketim_degerleri> CNCWeekWattReport { get; set; }

        //public IEnumerable<tuketim_degerleri> CNCMonthLast6Watt { get; set; }

        public IEnumerable<tuketim_degerleri> CNCSecilenWattMN01 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCSecilenWattMN02 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCSecilenWattCMA03 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCSecilenWattCMA04 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCSecilenWattCMA05 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCSecilenWattCMA06 { get; set; }
        public IEnumerable<tuketim_degerleri> CNCSecilenWattCMA07 { get; set; }

        public List<Models.Class.TarihTuketimCNC> CNCSecilenWattCMA07L { get; set; }

        public IEnumerable<CNC_1> CncSecilenXSaat { get; set; }

        public IEnumerable<tuketim_degerleri> CNCSecilenXTuketim { get; set; }

        public List<double> CNCMonthLast6WattMN01 { get; set; }
        public List<double> CNCMonthLast6WattMN02 { get; set; }
        public List<double> CNCMonthLast6WattCMA03 { get; set; }
        public List<double> CNCMonthLast6WattCMA04 { get; set; }
        public List<double> CNCMonthLast6WattCMA05 { get; set; }
        public List<double> CNCMonthLast6WattCMA06 { get; set; }
        public List<double> CNCMonthLast6WattCMA07 { get; set; }


        public List<string> CncMonthLast6 { get; set; }

        public IEnumerable<CNC_1> SecilenSaatler { get; set; }
        public IEnumerable<tuketim_degerleri> SecilenTuketim { get; set; }
    }
}