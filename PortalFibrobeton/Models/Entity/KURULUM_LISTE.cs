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
    
    public partial class KURULUM_LISTE
    {
        public long ID { get; set; }
        public string FORM_ADI { get; set; }
        public string LISTE_ADI { get; set; }
        public string LISTE_BASLIK { get; set; }
        public string ALAN_DATA { get; set; }
        public string FILTRE_DATA { get; set; }
        public string SIRALA_ALAN { get; set; }
        public Nullable<int> SATIR_YUKSEKLIK { get; set; }
        public Nullable<bool> FILTRELE { get; set; }
        public string FONT { get; set; }
        public Nullable<bool> OTO_BOY { get; set; }
        public Nullable<long> BOY_EN_AZ { get; set; }
        public Nullable<long> BOY_EN_YUK { get; set; }
        public Nullable<bool> RENKLENDIR { get; set; }
        public string RENKLENDIR_RENK { get; set; }
    }
}
