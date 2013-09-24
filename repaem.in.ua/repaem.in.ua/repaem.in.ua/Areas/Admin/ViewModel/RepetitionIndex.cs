using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class RepetitionIndex
	{
		public IEnumerable<Repetition> WaitingToApproveRepetitions { get; set; }

		public IEnumerable<Repetition> ApprovedRepetitions { get; set; } 

		public IEnumerable<Repetition> PastRepetitions { get; set; }

		public IEnumerable<Repetition> CancelledRepetitions { get; set; }
	}
}