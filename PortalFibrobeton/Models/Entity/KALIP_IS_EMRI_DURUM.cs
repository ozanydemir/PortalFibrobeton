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
    
    public partial class KALIP_IS_EMRI_DURUM
    {
        public long ID { get; set; }
        public Nullable<long> KALIP_IS_ID { get; set; }
        public string OLUSTURAN { get; set; }
        public Nullable<System.DateTime> OLUSTURMA_TARIH { get; set; }
        public string KALIP_DURUM { get; set; }
        public string DURUM_ACIKLAMASI { get; set; }
        public Nullable<long> EK_ID { get; set; }
        public string EK_TURU { get; set; }
        public Nullable<long> KALIP_HAZIRLAMA_SAYI { get; set; }
        public string MEV_IS_EMRI_DURUMU { get; set; }
    }
}
