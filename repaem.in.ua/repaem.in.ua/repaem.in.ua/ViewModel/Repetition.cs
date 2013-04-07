using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Repetition
    {
        public TimeRange Time { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public Status Status { get; set; }
    }

    public enum Status 
    {
        Ordered, Aprooved, Constant
    }
}