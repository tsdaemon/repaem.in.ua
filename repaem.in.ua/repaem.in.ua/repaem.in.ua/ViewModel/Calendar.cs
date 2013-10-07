using System;
using System.Collections.Generic;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   ВьюМодел Календаря для конкретної кімнати
	/// </summary>
	public class Calendar
	{
		public Calendar()
		{
			Events = new List<Repetition>();
			CurrentDate = DateTime.Today;
		}

		/// <summary>
		///   Ід кімнати
		/// </summary>
		public int RoomId { get; set; }

		public string RoomName { get; set; }

		public bool ManagerMode { get; set; }

		/// <summary>
		///   Список репетицій в цій кімнаті
		/// </summary>
		public List<Repetition> Events { get; set; }

		/// <summary>
		///   Дата, на яку буде встановлений календар
		/// </summary>
		public DateTime CurrentDate { get; set; }
	}
}