//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class RepBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public int ManagerId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
