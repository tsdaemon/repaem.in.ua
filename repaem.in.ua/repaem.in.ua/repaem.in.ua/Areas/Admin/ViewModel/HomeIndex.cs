using aspdev.repaem.ViewModel;
using System.Collections.Generic;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class HomeIndex
	{
		public List<Comment> Comments { get; set; }
		public List<Repetition> NewRepetitions { get; set; }
	}
}