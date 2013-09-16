using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел ціни
    /// </summary>
		[MetadataType(typeof(Price))]
    public class PriceMetadata
    {
      public int Id { get; set; }

			[Display(Name = "Начало"), Required]
      public int StartTime { get; set; }

			[Display(Name = "Окончание"), Required]
      public int EndTime { get; set; }

			[Display(Name = "Цена"), Required]
      public int Price { get; set; }

			public int RoomId { get; set; }
    }
}