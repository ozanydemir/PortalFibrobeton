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
    
    public partial class KALIP_IS_EMRI
    {
        public long ID { get; set; }
        public string IS_EMRINI_VEREN { get; set; }
        public Nullable<System.DateTime> OLUSTURULMA_TARIH { get; set; }
        public Nullable<System.DateTime> IS_EMRI_TARIH { get; set; }
        public string IS_EMRI_BARKOD { get; set; }
        public string POZ_NO { get; set; }
        public string KALIP_ADI { get; set; }
        public Nullable<long> KALIP_BOY { get; set; }
        public Nullable<long> KALIP_EN { get; set; }
        public Nullable<double> KALIP_M2 { get; set; }
        public string DOKUM_SEKLI { get; set; }
        public Nullable<long> DOKUM_MIKTAR { get; set; }
        public string DOKUM_RENGI { get; set; }
        public string MALZEME_YUZEYI { get; set; }
        public string IS_EMRI_ACIKLAMASI { get; set; }
        public string K_MODEL_BICAK_K_DURUM { get; set; }
        public string K_MODEL_BICAK_K_EDEN { get; set; }
        public string K_MODEL_BICAK_K_ACIKLAMA { get; set; }
        public Nullable<System.DateTime> K_MODEL_BICAK_K_TARIH { get; set; }
        public string K_KALIP_OLCULERI_K_DURUM { get; set; }
        public string K_KALIP_OLCULERI_K_ACIKLAMA { get; set; }
        public string K_KALIP_OLCULERI_K_EDEN { get; set; }
        public Nullable<System.DateTime> K_KALIP_OLCULERI_K_TARIH { get; set; }
        public string K_CAPRAZ_KONTROL_ACIKLAMA { get; set; }
        public string K_CAPRAZ_KONTROL_DURUM { get; set; }
        public string K_CAPRAZ_KONTROL_EDEN { get; set; }
        public Nullable<System.DateTime> K_CAPRAZ_KONTROL_TARIH { get; set; }
        public string K_YUZEY_ACI_K_DURUM { get; set; }
        public string K_YUZEY_ACI_K_ACIKLAMA { get; set; }
        public string K_YUZEY_ACI_K_EDEN { get; set; }
        public Nullable<System.DateTime> K_YUZEY_ACI_K_TARIH { get; set; }
        public string K_SOKUM_TAKIM_KENAR_DURUM { get; set; }
        public string K_SOKUM_TAKIM_KENAR_ACIKLAMA { get; set; }
        public string K_SOKUM_TAKIM_KENAR_K_EDEN { get; set; }
        public Nullable<System.DateTime> K_SOKUM_TAKIM_KENAR_K_TARIH { get; set; }
        public string K_SON_KONTROL_DURUM { get; set; }
        public string K_SON_KONTROL_ACIKLAMA { get; set; }
        public string K_SON_KONTROL_EDEN { get; set; }
        public Nullable<System.DateTime> K_SON_KONTROL_TARIH { get; set; }
        public Nullable<bool> KY_DUZ { get; set; }
        public Nullable<bool> KY_MKT_TEKSTURLU { get; set; }
        public Nullable<bool> KY_POLY_TEKSTURLU { get; set; }
        public Nullable<bool> KY_KAU_TEKSTURLU { get; set; }
        public Nullable<bool> KY_DIGER { get; set; }
        public string KY_DIGER_ADI { get; set; }
        public string IS_EMRI_DURUMU { get; set; }
        public Nullable<double> KB_POLYESTER_MIKTAR { get; set; }
        public Nullable<double> KB_POLYESTER_FIYAT { get; set; }
        public Nullable<double> KB_KAUCUK_MIKTAR { get; set; }
        public Nullable<double> KB_KAUCUK_FIYAT { get; set; }
        public Nullable<double> KB_CAMELYAF_MIKTAR { get; set; }
        public Nullable<double> KB_CAMELYAF_FIYAT { get; set; }
        public Nullable<double> KB_JETKOL_MIKTAR { get; set; }
        public Nullable<double> KB_JETKOL_FIYAT { get; set; }
        public Nullable<double> KB_STRAFOR_MIKTAR { get; set; }
        public Nullable<double> KB_STRAFOR_FIYAT { get; set; }
        public Nullable<double> KB_SILIKON_MIKTAR { get; set; }
        public Nullable<double> KB_SILIKON_FIYAT { get; set; }
        public Nullable<double> KB_ALCI_MIKTAR { get; set; }
        public Nullable<double> KB_ALCI_FIYAT { get; set; }
        public Nullable<double> KB_CELIK_SAC_MIKTAR { get; set; }
        public Nullable<double> KB_CELIK_SAC_FIYAT { get; set; }
        public Nullable<double> KB_CELIK_PROFIL_MIKTAR { get; set; }
        public Nullable<double> KB_CELIK_PROFIL_FIYAT { get; set; }
        public Nullable<double> KB_KOSEBENT_MIKTAR { get; set; }
        public Nullable<double> KB_KOSEBENT_FIYAT { get; set; }
        public Nullable<double> KB_LAMA_MIKTAR { get; set; }
        public Nullable<double> KB_LAMA_FIYAT { get; set; }
        public Nullable<double> KB_MDF10_MIKTAR { get; set; }
        public Nullable<double> KB_MDF10_FIYAT { get; set; }
        public Nullable<double> KB_MDF18_MIKTAR { get; set; }
        public Nullable<double> KB_MDF18_FIYAT { get; set; }
        public Nullable<double> KB_MDF33_MIKTAR { get; set; }
        public Nullable<double> KB_MDF33_FIYAT { get; set; }
        public string KB_DIGER1_ADI { get; set; }
        public Nullable<double> KB_DIGER1_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER1_FIYAT { get; set; }
        public string KB_DIGER2_ADI { get; set; }
        public Nullable<double> KB_DIGER2_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER2_FIYAT { get; set; }
        public string KB_DIGER3_ADI { get; set; }
        public Nullable<double> KB_DIGER3_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER3_FIYAT { get; set; }
        public string KB_DIGER4_ADI { get; set; }
        public Nullable<double> KB_DIGER4_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER4_FIYAT { get; set; }
        public string KB_DIGER5_ADI { get; set; }
        public Nullable<double> KB_DIGER5_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER5_FIYAT { get; set; }
        public string KB_DIGER6_ADI { get; set; }
        public Nullable<double> KB_DIGER6_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER6_FIYAT { get; set; }
        public string KB_DIGER7_ADI { get; set; }
        public Nullable<double> KB_DIGER7_MIKTAR { get; set; }
        public Nullable<double> KB_DIGER7_FIYAT { get; set; }
        public string KALIP_DURUMU { get; set; }
        public string KALIP_NO { get; set; }
        public Nullable<System.DateTime> KALIP_HAZIR_OL_TARIH { get; set; }
        public Nullable<System.DateTime> ONG_DOKUM_BASLAMA_TARIH { get; set; }
        public Nullable<long> DOKUM_OMRU { get; set; }
        public Nullable<long> POZ_ID { get; set; }
        public string DOKUM_HOLU { get; set; }
        public Nullable<long> KAL_DOK_SAY { get; set; }
        public Nullable<long> PLAN_DOK_SAY { get; set; }
        public string ANA_POZ_PROJE_ADI { get; set; }
        public Nullable<bool> DOKUM_BIT_SEVK_GONDER { get; set; }
        public Nullable<bool> BETON_DOKUM { get; set; }
        public Nullable<bool> ALCI_DOKUM { get; set; }
        public Nullable<bool> POLYESTER_DOKUM { get; set; }
        public Nullable<bool> KALIP_HAZIRLAMA_ACIK { get; set; }
        public Nullable<long> KALIP_HAZIRLAMA_SAYI { get; set; }
        public Nullable<long> DEPO_ID { get; set; }
        public string DEPO_ADI { get; set; }
        public string RAF_KONUM { get; set; }
    }
}