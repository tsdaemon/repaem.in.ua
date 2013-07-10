using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел замовлення бази
    /// </summary>
    public class RepBaseBook
    {
        public int RepBaseId { get; set; }

        [ReadOnly(true), DisplayName("База")]
        public string RepBaseName { get; set; }

        [DisplayName("Комната")]
        public Dictionary Room { get; set; }
        [DisplayName("Дата"), DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DisplayName("Время")]
        public TimeRange Time { get; set; }

        public RepBaseBook()
        {
            Room = new Dictionary();
        }
    }
}