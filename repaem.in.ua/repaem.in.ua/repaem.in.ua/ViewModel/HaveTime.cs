using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел для отображения, когда на базе есть свободное время, расчитывается для ближайшей неделе
    /// </summary>
    public class HaveTime
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public int TimeStart { get; set; }

        public int TimeStop { get; set; }

        public DateTime Date { get; set; }
    }
}