using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Infrastructure;
using aspdev.repaem.Models.Data;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class RoomEdit
	{
		public int Id { get; set; }

		public int ManagerId { get; set; }

		[Display(Name = "Название"), Length(Max=512, Min=3)]
		public string Name { get; set; }

		[Display(Name = "Описание"), DataType(DataType.MultilineText), Length(Max = 4000)]
		public string Description { get; set; }

		[Display(Name = "Репетиционная база")]
		public int RepBaseId { get; set; }
		public List<SelectListItem> RepBases { get; set; }

		[Display(Name = "Фотографии")]
		public PhotosEdit Photos { get; set; }

		[Display(Name = "Использовать сложную цену")]
		public bool ComplexPrice { get; set; }

		[Display(Name = "Цены")]
		public IEnumerable<Price> Prices { get; set; }

		[Display(Name = "Цена"), DataType(DataType.Currency)]
		public int? Price { get; set; }

		public RoomEdit()
		{
		}
	}
}