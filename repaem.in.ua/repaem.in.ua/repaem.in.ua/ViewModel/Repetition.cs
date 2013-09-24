using System;
using System.ComponentModel.DataAnnotations;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   Репетиція
	/// </summary>
	public class Repetition
	{
		public int Id { get; set; }

		//Вообще эти четыре поля нужно было сразу добавить и с помощью них выводить информацию о репетиции
		//вместо того, что бы юзать поле Name
		//TODO: refactor it
		public string RepBaseName { get; set; }

		public string RoomName { get; set; }

		public string BandName { get; set; }

		//Кто заказал
		public string UserName { get; set; }

		public string UserPhone { get; set; }

		//Поля тупо для того, что бы вытащить значения из базы
		public int TimeStart { get; set; }

		public int TimeEnd { get; set; }

		[Display(Name = "Время")]
		public TimeRange Time { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Дата")]
		public DateTime Date { get; set; }

		//Когда этот класс используется в списке репетиций базы - Имя того, кто заказал или группы
		//Список репетиций музыканта - Имя базы
		public string Name { get; set; }

		public Status Status { get; set; }

		[Display(Name = "Стоимость")]
		public int Sum { get; set; }

		[Display(Name = "Комментарии")]
		public string Comment { get; set; }

		public Repetition()
		{
			Time = new TimeRange();
		}
	}

	public enum Status
	{
		ordered, //только заказли
		approoved, //админы подтвердили
		cancelled //отменили
	}
}