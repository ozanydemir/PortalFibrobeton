using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.HammaddeTakip
{
    public class planlananDokumRaporuHammadde
    {
        public string PozNo { get; set; }

        public DateTime ? DokumTarih { get; set; }

        public string ProjeAdi { get; set; }

        public double ? DokumM2 { get; set; }

        public long ProjeID { get; set; }

    }
}