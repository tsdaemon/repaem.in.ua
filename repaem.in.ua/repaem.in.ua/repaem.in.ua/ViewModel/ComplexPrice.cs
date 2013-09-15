using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел складної ціни
    /// </summary>
    public class ComplexPrice
    {
      public int Id { get; set; }

			[Display(Name = "Начало")]
      public int StartTime { get; set; }

			[Display(Name = "Окончание")]
      public int EndTime { get; set; }

			[Display(Name = "Цена")]
      public int Price { get; set; }
    }
}