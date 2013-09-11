using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class RepaemItIsPastException : RepaemException
    {
        public RepaemItIsPastException(string msg)
            : base(msg) 
        {
            ModelKey = "Date";
        }
    }
}