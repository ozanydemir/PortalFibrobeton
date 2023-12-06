using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class
{
    public class uretimRaporuModelHammadde
    {
        public int Count { get; set; }
        public double TotalBirimKG { get; set; }

        public double TotalDokumM2 { get; set; }

        public string ProjeAdi { get; set;}

        public long ProjeID { get; set; }

        public string PozAdi { get; set; }
    }
}