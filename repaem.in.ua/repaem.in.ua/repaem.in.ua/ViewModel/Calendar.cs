using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Calendar
    {
        public int RoomId { get; set; }

        public List<Repetition> Events { get; set; }

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