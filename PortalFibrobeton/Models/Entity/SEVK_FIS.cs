//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortalFibrobeton.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class SEVK_FIS
    {
        public long ID { get; set; }
        public string OLUSTURAN { get; set; }
        public Nullable<System.DateTime> OLUSTURMA_TARIH { get; set; }
        public Nullable<System.DateTime> PLAN_SEVK_TARIH { get; set; }
        public string SEVK_NO { get; set; }
        public string SEVK_DURUMU { get; set; }
        public Nullable<long> TALEP_FIS_ID { get; set; }
        public string IRSALIYE_NO { get; set; }
        public string ARAC_PLAKA { get; set; }
        public string SOFOR_ADI { get; set; }
        public string SOFOR_TEL { get; set; }
        public Nullable<double> SEVK_POZ_MIKTAR { get; set; }
        public Nullable<double> SEVK_TON { get; set; }
        public Nullable<long> PROJE_ID { get; set; }
        public string ARAC_TIPI { get; set; }
        public Nullable<double> ARAC_EN { get; set; }
        public Nullable<double> ARAC_BOY { get; set; }
        public Nullable<double> ARAC_MAK_TON { get; set; }
        public string YUKLEYEN { get; set; }
        public Nullable<System.DateTime> YUKLEME_ZAMANI { get; set; }
        public string SEVKEDEN_FIRMA { get; set; }
        public Nullable<long> SEVKEDEN_FIRMA_ID { get; set; }
        public Nullable<double> SEVK_TUTARI { get; set; }
        public string SEVK_FIS_ACIKLAMA { get; set; }
        public Nullable<System.DateTime> ARAC_GIRIS_ZAMAN { get; set; }
        public Nullable<System.DateTime> ARAC_CIKIS_ZAMAN { get; set; }
        public Nullable<double> ILK_TARTIM { get; set; }
        public Nullable<double> SON_TARTIM { get; set; }
        public Nullable<System.DateTime> YUKLEME_BASLAMA_ZAMANI { get; set; }
        public string YUKLEME_SEFI { get; set; }
        public string SANTIYE_KODU { get; set; }
        public Nullable<long> SANTIYE_ID { get; set; }
        public Nullable<bool> SANTIYE_SEVK_KABUL { get; set; }
        public string SANTIYE_SEVK_KABUL_EDEN { get; set; }
        public Nullable<System.DateTime> SANTIYE_SEVK_KABUL_TARIH { get; set; }
        public string SEVK_KABUL_ACIKLAMA_FIS { get; set; }
        public Nullable<double> SEVK_PLAN_POZ_MIKTAR { get; set; }
        public Nullable<double> SEVK_KALAN_POZ_MIKTAR { get; set; }
        public string DORSE_PLAKA_NO { get; set; }
        public string KONTEYNER_NO { get; set; }
        public Nullable<double> ARAC_YUKLEME_SURE { get; set; }
        public Nullable<double> ARAC_FABRIKADA_KALMA_SURE { get; set; }
        public Nullable<double> INDIR_POZ_MIKTAR { get; set; }
        public Nullable<double> KALAN_INDIR_POZ_MIKTAR { get; set; }
        public Nullable<double> SEVK_M2 { get; set; }
        public Nullable<double> MONTAJ_POZ_MIKTAR { get; set; }
        public Nullable<double> KALAN_MONTAJ_POZ_MIKTAR { get; set; }
        public Nullable<double> SEVK_KG { get; set; }
    }
}
