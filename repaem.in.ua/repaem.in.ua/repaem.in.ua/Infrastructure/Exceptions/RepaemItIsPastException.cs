using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class RepaemItIsPastException : RepaemException
    {
        const string msg = "Не пытайтесь обмануть время";

        public RepaemItIsPastException()
            : base(msg) 
        {
            ModelKey = "Date";
        }
    }
}