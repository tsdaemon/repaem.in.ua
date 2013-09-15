using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class RoomListItem
	{
		public int Id { get; set; }

		[Display(Name = "Название")]
		public string Name { get; set; }

		[Display(Name = "Название комнаты")]
		public string RepBaseName { get; set; }

		[Display(Name = "Цена")]
		public int Price { get; set; }
	}
}