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
    
    public partial class AYARLAR_DOKUM_ROTA
    {
        public long ID { get; set; }
        public Nullable<bool> AKTIF { get; set; }
        public Nullable<long> ROTA_SIRA { get; set; }
        public string ISLEM_TURU { get; set; }
        public string ISLEM_ADI { get; set; }
        public string ISLEM_ONAY_ALAN { get; set; }
        public string ISLEM_ONAY_VEREN_ALAN { get; set; }
        public string ISLEM_ONAY_TARIH_ALAN { get; set; }
        public string EK_ISLEM_ADI { get; set; }
    }
}
