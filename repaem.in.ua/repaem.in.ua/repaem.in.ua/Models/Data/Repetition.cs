
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
    
public partial class Repetition
{

    public int Id { get; set; }

    public System.DateTime TimeStart { get; set; }

    public int MusicianId { get; set; }

    public double Sum { get; set; }

    public int RepBaseId { get; set; }

    public int RoomId { get; set; }

    public System.DateTime TimeEnd { get; set; }

    public string Comment { get; set; }

    public byte Status { get; set; }

}

}