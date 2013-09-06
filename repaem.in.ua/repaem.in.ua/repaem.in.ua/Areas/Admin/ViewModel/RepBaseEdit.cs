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

		[Display(Name = "Адрес"), DataType(DataType.MultilineText)]
		public string Address { get; set; }

		//[Display(Name = "Город"), DataType(DataType.MultilineText)]
		//public int CityId
		//{
		//	get { return City.Value; }
		//	set { City.Value = value; }
		//}
		//public Dictionary City { get; set; }

		//[Display(Name = "Район"), DataType(DataType.MultilineText)]
		//public int DistinctId {
		//	get { return Distinct.Value; }
		//	set { Distinct.Value = value; }
		//}
		//public Dictionary Distinct { get; set; }

		public string CityName { get; set; }

		public string DistinctName { get; set; }

		public double Lat { get; set; }

		public double Long { get; set; }

		public GoogleMap Map { get; set; }

		[Display(Name = "Фотографии")]
		public PhotosEdit Photos { get; set; }

		[Display(Name = "Комнаты")]
		public IEnumerable<Room> Rooms { get; set; }

		public RepBaseEdit()
		{
			//City = new Dictionary();
			//Distinct = new Dictionary();
		}
	}
}