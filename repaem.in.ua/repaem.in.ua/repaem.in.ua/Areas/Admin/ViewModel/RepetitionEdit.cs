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

		[Display(Name = "Телефон музыканта")]
		[Required(ErrorMessage = "Требуется номер телефона!")]
		[RegularExpression("\\+[0-9]{11,14}$", ErrorMessage = "Введите телефон в международном формате +38xxxyyyyyyy")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[Display(Name = "Комната")]
		public int RoomId { get; set; }
		public List<SelectListItem> Rooms { get; set; }

		[Display(Name = "Репетиционная база")]
		public int RepBaseId { get; set; }
		public List<SelectListItem> RepBases { get; set; }

		[DataType(DataType.MultilineText)]
		[Display(Name = "Комментарий")]
		public string Comment { get; set; }

		[Display(Name = "Время"), repaem.ViewModel.Range(0, 24)]
		public TimeRange Time { get; set; }

	}
}