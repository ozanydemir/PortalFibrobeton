﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TaramaEntities : DbContext
    {
        public TaramaEntities()
            : base("name=TaramaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tbl_Log> Tbl_Log { get; set; }
        public virtual DbSet<Tbl_Taramalar> Tbl_Taramalar { get; set; }
        public virtual DbSet<Tbl_Users> Tbl_Users { get; set; }
    }
}
