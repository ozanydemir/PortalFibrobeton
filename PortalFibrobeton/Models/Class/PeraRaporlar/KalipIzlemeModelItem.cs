using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.PeraRaporlar
{
    public class KalipIzlemeModelItem
    {
        public string PROJE_KODU { get; set; }
        public string PROJE_ADI { get; set; }
        public string PROJE_MUDURU { get; set; }
        public string PROJE_YONETICISI { get; set; }
        public string IS_EMRI_DURUMU { get; set; }
        public string KALIP_DURUMU { get; set; }
        public string IS_EMRI_BARKODU { get; set; }
        public DateTime? IS_EMRI_TARIH { get; set; }
        public DateTime? IE_ONG_DOK_BASLAMA_TARIHI { get; set; }
        public DateTime? KALIP_HAZIR_OL_TARIH { get; set; }
        public string KALIP_TIPI { get; set; }
        public int? SIRA { get; set; }
        public string ANA_POZ { get; set; }
        public string POZ_NO { get; set; }
        public string MALZEME_ADI { get; set; }
        public double? EN { get; set; }
        public double? BOY { get; set; }
        public bool? POZ_IPTAL { get; set; }
        public double? KALIP_YUZEY_M2 { get; set; }
        public double? KALIP_TOP_DOK_ADET { get; set; }
        public double? KALIP_DOKUM_OMRU_ADET { get; set; }
        public bool? HOLD { get; set; }
        public string MALZEME_GRUP { get; set; }
        public string MALZEME_CINSI { get; set; }
        public string KALIPHANE { get; set; }
        public string KALIP_ISLEM_TURU { get; set; }
        public string KALIPHANE_ISLEM_ADI { get; set; }

    }
}