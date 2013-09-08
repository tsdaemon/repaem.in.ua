using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class RepaemException : Exception
    {
	    public RepaemException(string message) : base(message)
	    {
	    }

        public string ModelKey { get; protected set; }
    }
}