using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class HomeIndex
	{
		public List<dynamic> Comments { get; set; }
		public List<dynamic> NewRepetitions { get; set; }
	}
}