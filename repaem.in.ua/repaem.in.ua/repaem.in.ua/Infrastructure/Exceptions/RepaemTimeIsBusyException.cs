using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class RepaemTimeIsBusyException : RepaemException
    {
        const string msg = "Это время уже занято! Попробуйте другое!";

        public RepaemTimeIsBusyException() : base(msg) 
        {
            ModelKey = "Time";
        }
    }
}