using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
	public class Comments : List<Comment>
	{
		public string RepBaseName { get; set; }

		public int RepBaseId { get; set; }

		public Comments()
		{
			
		}

		public Comments(IEnumerable<Comment> ls) : base(ls)
		{
			
		}
	}
}