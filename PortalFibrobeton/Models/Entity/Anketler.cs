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
    
    public partial class Anketler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Anketler()
        {
            this.AnketSonuclar = new HashSet<AnketSonuclar>();
        }
    
        public int AnketID { get; set; }
        public string AnketiOlusturan { get; set; }
        public System.DateTime AnketiOlusturmaTarihi { get; set; }
        public string SablonDurum { get; set; }
        public string AnketAdi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnketSonuclar> AnketSonuclar { get; set; }
    }
}
