using System;
using System.ComponentModel.DataAnnotations;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	/// ViewModel для отображения и редактирования фильтра репетиционных баз
	/// </summary>
	public class RepBaseFilter
	{
		public enum DisplayType
		{
			inline,
			square
		}

		public RepBaseFilter()
		{
			City = new Dictionary();
			Distinct = new Dictionary();
			Date = DateTime.Today;
			Time = new TimeRange(4, 20);
			Price = new Range(25, 75);
		}

		[Display(Name = "Название базы")]
		public string Name { get; set; }

		[Display(Name = "Город")]
		public Dictionary City { get; set; }

		[Display(Name = "Район")]
		public Dictionary Distinct { get; set; }

		[Display(Name = "Дата"), DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[Display(Name = "Время"), Range(0, 24)]
		public TimeRange Time { get; set; }

		[Display(Name = "Стоимость"), Range(0, 100)]
		public Range Price { get; set; }

		public DisplayType DisplayTpe { get; set; }
	}
}