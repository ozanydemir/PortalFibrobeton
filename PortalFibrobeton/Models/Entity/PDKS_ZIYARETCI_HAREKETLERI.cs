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
    
    public partial class PDKS_ZIYARETCI_HAREKETLERI
    {
        public long ID { get; set; }
        public Nullable<System.DateTime> ZAMAN { get; set; }
        public string GUN { get; set; }
        public Nullable<long> ZIYARETCI_ID { get; set; }
        public Nullable<System.DateTime> BILGI_AKTARIM_TARIH { get; set; }
        public string BILGI_AKTARIM_NO { get; set; }
        public Nullable<long> CIHAZ_ID { get; set; }
    }
}
