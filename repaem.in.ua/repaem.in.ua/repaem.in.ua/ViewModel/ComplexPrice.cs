using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел складної ціни
    /// </summary>
    public class ComplexPrice
    {
        public int Id { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public int Price { get; set; }
    }
}