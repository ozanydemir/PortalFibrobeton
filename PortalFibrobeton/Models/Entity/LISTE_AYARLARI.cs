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
    
    public partial class LISTE_AYARLARI
    {
        public long ID { get; set; }
        public Nullable<long> KULLANICI_ID { get; set; }
        public string TURU { get; set; }
        public Nullable<bool> GOSTER { get; set; }
        public string BASLIK { get; set; }
        public string ALAN { get; set; }
        public Nullable<long> BOY { get; set; }
        public Nullable<bool> KULLANICIYA_GOSTERME { get; set; }
        public Nullable<long> SIRA { get; set; }
    }
}