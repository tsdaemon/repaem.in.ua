using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Message
    {
        public string Caption { get; set; }

        public string Text { get; set; }

        public RepaemColor Color { get; set; }

        public Message()
        {
            Color = new RepaemColor("blue2");
        }
    }
}