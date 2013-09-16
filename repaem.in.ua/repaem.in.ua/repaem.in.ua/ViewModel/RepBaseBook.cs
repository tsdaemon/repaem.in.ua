using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   ВьюМодел замовлення бази
	/// </summary>
	public class RepBaseBook
	{
		public int RepBaseId { get; set; }

		[ReadOnly(true), DisplayName("База")]
		public string RepBaseName { get; set; }

		[DisplayName("Комната")]
		public int RoomId { get; set; }
		public List<SelectListItem> Rooms { get; set; }

		[DisplayName("Дата"), DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[DisplayName("Время"), Range(0, 24)]
		public TimeRange Time { get; set; }

		[DisplayName("Комментарий"), DataType(DataType.MultilineText)]
		public string Comment { get; set; }

		public RepBaseBook()
		{
		}
	}
}