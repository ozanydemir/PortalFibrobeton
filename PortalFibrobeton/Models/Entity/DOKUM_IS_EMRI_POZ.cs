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
    
    public partial class DOKUM_IS_EMRI_POZ
    {
        public long ID { get; set; }
        public Nullable<long> KALIP_IS_ID { get; set; }
        public Nullable<long> DOKUM_IS_ID { get; set; }
        public Nullable<long> PROJE_ID { get; set; }
        public Nullable<long> POZ_ID { get; set; }
        public string DOKUM_POZ_BARKOD { get; set; }
        public Nullable<double> SIRA { get; set; }
        public Nullable<double> DOKULECEK_ADET { get; set; }
        public Nullable<double> DOKULEN_ADET { get; set; }
        public Nullable<double> KALAN_DOKUM_ADET { get; set; }
        public Nullable<double> PLAN_DOKUM_ADET { get; set; }
        public Nullable<double> KALAN_DOKUM_PLAN_ADET { get; set; }
    }
}