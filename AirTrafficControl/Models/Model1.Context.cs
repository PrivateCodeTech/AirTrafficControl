﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirTrafficControl.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Centre> Centres { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Convention> Conventions { get; set; }
        public DbSet<ConventionDocument> ConventionDocuments { get; set; }
        public DbSet<ConventionStage> ConventionStages { get; set; }
        public DbSet<ConventionType> ConventionTypes { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LicensesType> LicensesTypes { get; set; }
        public DbSet<PaymentReceipt> PaymentReceipts { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<State> States { get; set; }
    }
}
