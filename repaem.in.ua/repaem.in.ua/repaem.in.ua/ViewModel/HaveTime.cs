using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class HaveTime
    {
        public string RoomName { get; set; }

        public int TimeStart { get; set; }

        public int TimeStop { get; set; }

        public DateTime Date { get; set; }
    }
}