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
    
    public partial class PROJE_KART
    {
        public long ID { get; set; }
        public string PROJE_DURUMU { get; set; }
        public string PROJE_KODU { get; set; }
        public string PROJE_ADI { get; set; }
        public string PROJE_MUDURU { get; set; }
        public string PROJE_YONETICISI { get; set; }
        public string TEKLIF_NO { get; set; }
        public Nullable<System.DateTime> SOZLESME_TARIH { get; set; }
        public Nullable<System.DateTime> BASLAMA_TARIH { get; set; }
        public Nullable<System.DateTime> BITIS_TARIH { get; set; }
        public string PROJE_TURU { get; set; }
        public string PROJE_YERI { get; set; }
        public string PROJE_ADRES { get; set; }
        public string PROJE_ACIKLAMASI { get; set; }
        public string IL { get; set; }
        public string ILCE { get; set; }
        public Nullable<long> PROJE_GUN { get; set; }
        public Nullable<long> PROJE_GERCEK_GUN { get; set; }
        public string PROJE_GERCEK_GUN_LIST { get; set; }
        public Nullable<double> PROJE_SATIS_M2 { get; set; }
        public Nullable<long> PROJE_SATIS_MIKTAR { get; set; }
        public string SANTIYE_SEFI { get; set; }
        public Nullable<double> SOZLESME_M2 { get; set; }
        public Nullable<long> SOZLESME_M2_KONTROL_YUZDESI { get; set; }
        public Nullable<long> WEB_PANEL_YANLIS_SIFRE { get; set; }
        public string WEB_PANEL_KULLANICI_ADI { get; set; }
        public string WEB_PANEL_KULLANICI_SIFRE { get; set; }
        public string ULKE { get; set; }
        public string IS_DEVIR_ACIKLAMALARI { get; set; }
        public string SOZLESME_NO { get; set; }
        public string BIZDEN_ILGILI { get; set; }
        public string SANDIK_PALET { get; set; }
        public Nullable<bool> V_URUN_M2_KULLAN { get; set; }
        public Nullable<System.DateTime> V_URUN_M2_SEVK_TARIH { get; set; }
        public string V_URUN_M2_TARIH_ONCE { get; set; }
        public string V_URUN_M2_TARIH_SONRA { get; set; }
        public string MONTAJ_DETAY_PLAN_ACIKLAMA1 { get; set; }
        public string MONTAJ_DETAY_PLAN_ACIKLAMA2 { get; set; }
        public string SANTIYE_SORUMLUSU { get; set; }
        public Nullable<double> MONTAJ_PLAN_BASLANGIC { get; set; }
        public Nullable<System.DateTime> MONTAJ_BASLANMA_TARIH { get; set; }
        public Nullable<System.DateTime> MONTAJ_BITIS_TARIH { get; set; }
        public Nullable<long> MONTAJ_GUN { get; set; }
        public string MASTIK { get; set; }
        public string BOYA { get; set; }
        public Nullable<int> KUR_SURE_GUN { get; set; }
        public Nullable<bool> URUN_3D_TARAMA_ZORUNLU { get; set; }
        public Nullable<bool> KALIP_3D_TARAMA_ZORUNLU { get; set; }
        public Nullable<bool> MODEL_UYGUNLUK_KONTROL_ZORUNLU { get; set; }
        public Nullable<bool> KALITE_SON_ONAY_ZORUNLU { get; set; }
        public Nullable<bool> CATLAK_KONTROL_ZORUNLU { get; set; }
    }
}
