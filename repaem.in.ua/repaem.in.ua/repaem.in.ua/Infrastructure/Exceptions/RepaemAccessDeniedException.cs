using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepaemAccessDeniedException : RepaemException
	{
		public RepaemAccessDeniedException(string message) : base(message)
		{
		}
	}
}