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
    
    public partial class SIPARIS_PLAN
    {
        public long ID { get; set; }
        public Nullable<long> PROJE_ID { get; set; }
        public Nullable<System.DateTime> PLAN_TARIH { get; set; }
        public Nullable<double> PLAN_MIKTAR { get; set; }
        public string OLUSTURAN { get; set; }
        public Nullable<System.DateTime> OLUSTURMA_ZAMANI { get; set; }
        public string SON_DUZENLEME { get; set; }
        public Nullable<System.DateTime> SON_DUZENLEME_ZAMANI { get; set; }
    }
}
