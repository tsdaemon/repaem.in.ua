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
        int base_id;
        public int RepBaseId { get { return base_id; } set { base_id = value; LoadBaseValues(value); } }

        private void LoadBaseValues(int value)
        {
            //TODO: TO KCH загрузити назву бази та список її кімнат
        }

        [ReadOnly(true), DisplayName("База")]
        public string RepBaseName { get; private set; }

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