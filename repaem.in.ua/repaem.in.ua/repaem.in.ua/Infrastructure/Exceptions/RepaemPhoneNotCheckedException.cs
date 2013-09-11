using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepaemPhoneNotCheckedException : RepaemException
	{
		public RepaemPhoneNotCheckedException(string message) : base(message)
		{
			
		}
	}
}