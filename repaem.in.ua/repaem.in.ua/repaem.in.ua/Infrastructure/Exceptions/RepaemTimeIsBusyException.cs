using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class RepaemTimeIsBusyException : RepaemException
    {
        public RepaemTimeIsBusyException(string msg) : base(msg) 
        {
            ModelKey = "Time";
        }
    }
}