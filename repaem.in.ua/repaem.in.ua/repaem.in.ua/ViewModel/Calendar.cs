using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел Календаря для конкретної кімнати
    /// </summary>
    public class Calendar
    {
        /// <summary>
        /// Ід кімнати
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Список репетицій в цій кімнаті
        /// </summary>
        public List<Repetition> Events { get; set; }

        /// <summary>
        /// Дата, на яку буде встановлений календар
        /// </summary>
        public DateTime CurrentDate { get; set; }

        public Calendar()
        {
            Events = new List<Repetition>();
            CurrentDate = DateTime.Today;
        }

        public Calendar(bool demo):this()
        {
            Events.Add(new Repetition() { Date = new DateTime(2013, 4, 7), Name = "Час Ночи", Status = Status.approoved, Time = new TimeRange(15,17) });
            Events.Add(new Repetition() { Date = new DateTime(2013, 4, 7), Name = "Час Ночи", Status = Status.constant, Time = new TimeRange(17, 20) });
            CurrentDate = new DateTime(2013, 3, 22);
        }
    }
}