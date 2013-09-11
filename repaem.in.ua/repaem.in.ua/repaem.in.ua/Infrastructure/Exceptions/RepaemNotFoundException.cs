﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepaemNotFoundException : RepaemException
	{
		public string TableName { get; set; }
		public int ItemId { get; set; }

		public RepaemNotFoundException(string message) : base(message)
		{
		}
	}
}