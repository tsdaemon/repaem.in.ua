using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepaemRepetitionWrongStatusException : RepaemException
	{
		public Status Status { get; set; }

		public RepaemRepetitionWrongStatusException(string message) : base(message)
		{
		}

		public RepaemRepetitionWrongStatusException()
			: base("Неправильный статус репетиции!")
		{
		}
	}
}