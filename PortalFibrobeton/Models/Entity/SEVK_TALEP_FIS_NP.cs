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
    
    public partial class SEVK_TALEP_FIS_NP
    {
        public long ID { get; set; }
        public Nullable<long> PROJE_ID { get; set; }
        public Nullable<System.DateTime> PLAN_SEVK_TARIH { get; set; }
        public string TALEP_NO { get; set; }
        public string TALEP_DURUMU { get; set; }
        public string TALEP_ACIKLAMASI { get; set; }
        public Nullable<double> TALEP_TOPLAM_M2 { get; set; }
        public Nullable<double> TALEP_TOPLAM_ADET { get; set; }
        public Nullable<double> TALEP_TOPLAM_KG { get; set; }
        public string OLUSTURAN { get; set; }
        public Nullable<System.DateTime> OLUSTURMA_TARIH { get; set; }
    }
}