using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.PeraRaporlar
{
    public class UretimRaporuModelItem
    {
        public string PROJE_ADI { get; set; }
        public string PROJE_TURU { get; set; }
        public DateTime? SIPARIS_TARIH { get; set; }
        public string SIPARIS_NO { get; set; }
        public string POZ_NO { get; set; }
        public string MALZEME_ADI { get; set; }
        public string MALZEME_CINSI { get; set; }
        public double? EN { get; set; }
        public double? BOY { get; set; }
        public double? SON_SIPARIS_MIKTARI { get; set; }
        public double? KALAN_DOKUM { get; set; }
        public DateTime? DOKUM_TARIH { get; set; }
        public string HAT { get; set; }
        public double? DOKULEN_ADET { get; set; }
        public double? SIPARIS_METREKARE { get; set; } //Birim m2
        public double? DOKUM_SIPARIS_METREKARE { get; set; } // SIP M2
        public double? DOKUM_METREKARE { get; set; } // DOKUM_M2
        public double? DOK_DOKUM_M2 { get; set; } //DOK_M2
        public double? DOKUM_URUN_AGIRLIK { get; set; }
        public double? DOKUM_FRAME_AGIRLIK { get; set; }
        public string IS_EMRI_BARKODU { get; set; }
        public DateTime? IS_EMRI_TARIH { get; set; }
        public string BLOK_ADI { get; set; }
        public string CEPHE_ADI { get; set; }
        public string POZ_TURU { get; set; }
        public string DOKUM_EKIP { get; set; }

    }
}