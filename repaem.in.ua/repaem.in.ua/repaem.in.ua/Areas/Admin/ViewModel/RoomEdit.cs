using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using aspdev.repaem.Infrastructure;
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
		public Dictionary RepBase { get; set; }

		[Display(Name = "Фотографии")]
		public PhotosEdit Photos { get; set; }

		[Display(Name = "Сложная цена")]
		public bool ComplexPrice { get; set; }

		[Display(Name = "Цены")]
		public IEnumerable<ComplexPrice> Prices { get; set; }

		[Display(Name = "Цена")]
		public int Price { get; set; }

		public RoomEdit()
		{
		}
	}
}