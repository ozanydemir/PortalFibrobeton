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
    
    public partial class KALIP_IS_EMRI_KALIPHANE_BARKOD
    {
        public long ID { get; set; }
        public Nullable<long> KALIP_IS_ID { get; set; }
        public Nullable<long> ISLEM_ID { get; set; }
        public Nullable<long> KALIP_TASARIM_ID { get; set; }
        public string PARCA_BARKOD { get; set; }
        public Nullable<bool> TESLIM_ALINDI { get; set; }
        public string TESLIM_ALINDI_YAPAN { get; set; }
        public Nullable<System.DateTime> TESLIM_ALINDI_ZAMAN { get; set; }
    }
}
