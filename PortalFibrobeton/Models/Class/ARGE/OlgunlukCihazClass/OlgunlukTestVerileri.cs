using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.ARGE.OlgunlukCihazClass
{
    public class OlgunlukTestVerileri
    {
        public double Olgunluk { get; set; }
        public double BasincDayanim { get; set; }

        public double EgilmeDayanim { get; set; }
        public double Sicaklik { get; set; }

        public DateTime SensorTarih { get; set; }
    }
}