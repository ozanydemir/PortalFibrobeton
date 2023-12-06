using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PortalFibrobeton.Models.Class.ARGE.OlgunlukCihazClass
{
    public class OlgunlukCihazClass
    {
        //Aktif
        public TimeSpan ElapsedTime { get; set; }
        //Aktif
        public System.Timers.Timer Timer { get; set; }
        //Aktif
        public DateTime LastDataReceivedTime { get; set; }
        //Aktif
        public double TahminiKalanSure { get; set; }
        //Aktif
        public bool TestDurum { get; set; }
        //Aktif
        public double MinTemp { get; set; }
        public List<double> LastTenTemps { get; set; }

        public OlgunlukCihazClass()
        {
            LastTenTemps = new List<double>();
        }
    }

    
}