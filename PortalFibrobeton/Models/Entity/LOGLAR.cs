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
    
    public partial class LOGLAR
    {
        public long ID { get; set; }
        public string BILGI { get; set; }
        public string MODUL { get; set; }
        public Nullable<System.DateTime> TARIH { get; set; }
        public string KULLANICI_ADI { get; set; }
        public string ISLEM { get; set; }
        public Nullable<long> ISLEM_ID { get; set; }
    }
}
