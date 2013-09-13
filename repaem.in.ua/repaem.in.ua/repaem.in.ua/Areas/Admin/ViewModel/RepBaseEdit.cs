using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using aspdev.repaem.ViewModel;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class RepBaseEdit
	{
		public int Id { get; set; }

		public int ManagerId { get; set; }

		[Display(Name = "Название")]
		public string Name { get; set; }

		[Display(Name = "Описание"), DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[Display(Name = "Адрес")]
		public string Address { get; set; }

		[Display(Name = "Город")]
		public string CityName { get; set; }
		public int CityId { get; set; }

		public string DistinctName { get; set; }

		[DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false)]
		public double Lat { get; set; }
		[DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false)]
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