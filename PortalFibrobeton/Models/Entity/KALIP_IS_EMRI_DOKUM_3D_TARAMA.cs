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
    
    public partial class KALIP_IS_EMRI_DOKUM_3D_TARAMA
    {
        public long ID { get; set; }
        public Nullable<long> DOKUM_ID { get; set; }
        public Nullable<bool> BASLADI { get; set; }
        public Nullable<System.DateTime> BASLADI_ZAMAN { get; set; }
        public Nullable<bool> BITTI { get; set; }
        public Nullable<System.DateTime> BITTI_ZAMAN { get; set; }
        public string SONUC_ACIKLAMASI { get; set; }
        public string OLUSTURAN { get; set; }
        public Nullable<System.DateTime> OLUSTURMA_TARIH { get; set; }
    }
}
