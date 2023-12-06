using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalFibrobeton.Models.Class.PeraRaporlar
{
    public class SiparisIzlemeModelItem
    {
        public DateTime? SIPARIS_TARIH { get; set; }
        public DateTime? REVIZE_TARIH { get; set; }
        public string PROJE_ADI { get; set; }
        public string DOKUM_SEKLI { get; set; }
        public string HAT { get; set; }
        public string SIPARIS_TURU { get; set; }
        public string POZ_NO { get; set; }
        public string MALZEME_ADI { get; set; }
        public double? EN { get; set; }
        public double? BOY { get; set; }
        public string KALIP_SEKLI { get; set; }
        public string IS_EMRI_NO { get; set; }
        public string RENK { get; set; }
        public string MALZEME_YUZEYI { get; set; }
        public bool? MONTAJSIZ { get; set; }
        public DateTime? IS_EMRI_TARIH { get; set; }
        public string IS_EMRINI_VEREN { get; set; }
        public double? SATIS_BIRIM_M2 { get; set; }
        public double? DOKUM_BIRIM_M2 { get; set; }
        public double? MALZEME_BIRIM_KG { get; set; }
        public double? BIRIM_HACIM_M3 { get; set; }
        public double? FRAME_BIRIM_KG { get; set; }
        public double? STOK { get; set; }
        public double? TOPLAM { get; set; }
        public double? DOKULEN { get; set; }
        public double? KALAN { get; set; }
        public double? KALAN_SATIS_M2 { get; set; }
        public double? DOKULEN_SATIS_M2 { get; set; }
        public double? TOPLAM_SIPARIS_HACIM_M2 { get; set; }
        public double? KALAN_SIPARIS_HACIM_M2 { get; set; }
        public double? NAKLEDILEN_M2 { get; set; }
        public double? NAKLEDILEN_ADET { get; set; }
        public double? INDIRILEN_AD { get; set; }
        public double? INDIRILEN_M2 { get; set; }
        public double? MONTAJLANAN_AD { get; set; }
        public double? MONTAJLANAN_M2 { get; set; }
        public double? HURDA_KARANTINA_AD { get; set; }
        public double? HURDA_KARANTINA_M2 { get; set; }
        public double? KALAN_MONTAJ_AD { get; set; }
        public double? KALAN_MONTAJ_M2 { get; set; }
        public double? TOPLAM_SATIS_M2 { get; set; }
        public string YUZEY_ISLEM_1 { get; set; }
        public string YUZEY_ISLEM_2 { get; set; }
        public string YUZEY_ISLEM_3 { get; set; }
        public string YUZEY_ISLEM_4 { get; set; }
        public string YUZEY_ISLEM_5 { get; set; }
        public string REPORT_CODE { get; set; }
        public double? ONGORULEN_URUN_AGIRLIK { get; set; }
        public string BLOK_ADI { get; set; }
        public string CEPHE_ADI { get; set; }
        public string POZ_TURU { get; set; }
        public string KALIP_TASARIM_NO { get; set; }
        public string FRAME_DURUMU { get; set; }

    }
}