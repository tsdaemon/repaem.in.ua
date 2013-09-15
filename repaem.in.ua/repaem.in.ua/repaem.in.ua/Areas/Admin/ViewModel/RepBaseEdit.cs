using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using aspdev.repaem.ViewModel;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class RepBaseEdit
	{
		public int Id { get; set; }

		public int ManagerId { get; set; }

		[Display(Name = "Название"), Length(Max=512, Min=3)]
		public string Name { get; set; }

		[Display(Name = "Описание"), DataType(DataType.MultilineText), Length(Max = 4000)]
		public string Description { get; set; }

		[Display(Name = "Адрес"), Length(Max = 512)]
		public string Address { get; set; }

		[Display(Name = "Город"), Length(Max = 512)]
		public string CityName { get; set; }
		public int CityId { get; set; }

		public double Lat { get; set; }

		public double Long { get; set; }

		[Display(Name = "Карта", Description = "Отмечайте репетиционную базу на карте")]
		public GoogleMap Map { get; set; }

		[Display(Name = "Фотографии")]
		public PhotosEdit Photos { get; set; }

		[Display(Name = "Комнаты")]
		public IEnumerable<Room> Rooms { get; set; }

		public RepBaseEdit()
		{
		}
	}
}