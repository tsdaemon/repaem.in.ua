using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace aspdev.repaem.ViewModel
{
    public class RepBaseFilter
    {
        [Display(Name="Название")]
        public string Name { get; set; }

        [Display(Name="Город")]
        public int City { get; set; }

        public Dictionary<int, string> Cities { get; set; }

        [Display(Name = "Район")]
        public int Distinct { get; set; }

        public Dictionary<int, string> Distincts { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Время")]
        public Tuple<int, int> Time { get; set; }

        [Display(Name = "Деньги")]
        public Tuple<int, int> Price { get; set; }

        public RepBaseFilter()
        {
            Time = new Tuple<int, int>(4, 20);
            Price = new Tuple<int, int>(25, 75);

            Cities = new Dictionary<int, string>();
            Cities.Add(1, "Киев");
            Cities.Add(2, "Кременчуг");

            Distincts = new Dictionary<int, string>();
        }
    }
}