using aspdev.repaem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class HomePage
	{
		public string UserName { get; set; }

		public GoogleMap Map { get; set; }

		public List<RepBaseListItem2> RepBases { get; set; }

		public List<Repetition> NewRepetitions { get; set; }

		public bool UnpaidInvoice { get; set; }

		public HomePage()
		{
			Map = new GoogleMap();
			RepBases = new List<RepBaseListItem2>();
			NewRepetitions = new List<Repetition>();
		}
	}
}