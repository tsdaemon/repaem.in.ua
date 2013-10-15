using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepaemSmsException : RepaemException
	{

		public RepaemSmsException(string message)
			: base(message)
		{
		}
	}
}