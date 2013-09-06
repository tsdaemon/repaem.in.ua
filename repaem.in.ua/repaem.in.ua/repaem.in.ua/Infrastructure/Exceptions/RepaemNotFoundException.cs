using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepaemItemNotFoundException : RepaemException
	{
		public string TableName { get; set; }
		public int ItemId { get; set; }

		public RepaemItemNotFoundException(string message) : base(message)
		{
		}
	}
}