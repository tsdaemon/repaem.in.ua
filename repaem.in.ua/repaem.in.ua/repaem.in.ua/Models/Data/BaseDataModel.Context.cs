﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace aspdev.repaem.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BaseDataModelContainer : DbContext
    {
        public BaseDataModelContainer()
            : base("name=BaseDataModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Client> ClientSet { get; set; }
        public DbSet<City> CitySet { get; set; }
        public DbSet<Band> BandSet { get; set; }
        public DbSet<RepBase> RepBaseSet { get; set; }
        public DbSet<BlackList> BlackListSet { get; set; }
        public DbSet<AdditionalEquipment> AdditionalEquipmentSet { get; set; }
        public DbSet<Invoice> InvoiceSet { get; set; }
        public DbSet<Room> RoomSet { get; set; }
        public DbSet<Price> PriceSet { get; set; }
    }
}
