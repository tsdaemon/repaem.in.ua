using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace aspdev.repaem.ViewModel
{
    public class RepBaseFilter
    {
        [Display(Name="Название базы")]
        public string Name { get; set; }

        [Display(Name="Город")]
        public int City { get; set; }

        public Dictionary<int, string> Cities { get; set; }

        [Display(Name = "Район")]
        public int Distinct { get; set; }

        public Dictionary<int, string> Distincts { get; set; }

        [Display(Name = "Дата"), DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Время"), Range(0, 24)]
        public Range Time { get; set; }

        [Display(Name = "Стоимость"), Range(0, 100)]
        public Range Price { get; set; }

        public RepBaseFilter()
        {
            Time = new Range(4, 20);
            Price = new Range(25, 75);

            Cities = new Dictionary<int, string>();
            Cities.Add(1, "Киев");
            Cities.Add(2, "Кременчуг");

            Distincts = new Dictionary<int, string>();
        }
    }
}