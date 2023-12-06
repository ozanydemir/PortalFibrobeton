using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.HammaddeTakip
{
    public class birlesmisHammaddeRaporu
    {
        public string ProjeAdi { get; set; }

        public string PozNo { get; set; }

        public DateTime ? DokumTarihi { get; set; }

        public double ? KalıpM2 { get; set; }

        public double ? TahminiKutle { get; set; }

        public double ? TahminiFrameKutle { get; set; }

        public double ? ToplamKutle { get; set; }

        public double ? TamKovaSayisi { get; set; }
    }
}