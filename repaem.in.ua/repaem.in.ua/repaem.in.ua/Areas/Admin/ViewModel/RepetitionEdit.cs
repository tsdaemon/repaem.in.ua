using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class RepetitionEdit 
	{
		public int Id { get; set; }

		[Display(Name = "Стоимость"), DataType(DataType.Currency)]
		public int Sum { get; set; }

		[Display(Name = "Дата")]
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[Display(Name = "Комната")]
		public int RoomId { get; set; }
		public List<SelectListItem> Rooms { get; set; }

		public int RepBaseId { get; set; }

		[DataType(DataType.MultilineText)]
		[Display(Name = "Комментарий")]
		public string Comment { get; set; }

		[Display(Name = "Время"), repaem.ViewModel.Range(0, 24)]
		public TimeRange Time { get; set; }
	}
}