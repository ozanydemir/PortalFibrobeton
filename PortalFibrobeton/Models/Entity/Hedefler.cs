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
    
    public partial class Hedefler
    {
        public int ID { get; set; }
        public Nullable<long> ProjeID { get; set; }
        public string Cephe { get; set; }
        public Nullable<System.DateTime> HedefTarih { get; set; }
        public Nullable<double> PlanlananParcaSiparis { get; set; }
        public Nullable<double> PlanlananM2Siparis { get; set; }
        public Nullable<double> PlanlananParcaDokum { get; set; }
        public Nullable<double> PlanlananM2Dokum { get; set; }
        public Nullable<double> PlanlananParcaSevkiyat { get; set; }
        public Nullable<double> PlanlananM2Sevkiyat { get; set; }
    }
}