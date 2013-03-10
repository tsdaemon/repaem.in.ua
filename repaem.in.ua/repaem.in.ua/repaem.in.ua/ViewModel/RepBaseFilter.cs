using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
    public class RepBaseFilter
    {
        [Display(Name="Название базы")]
        public string Name { get; set; }

        [Display(Name="Город")]
        public int City { get; set; }

        public List<SelectListItem> Cities { get; set; }

        [Display(Name = "Район")]
        public int Distinct { get; set; }

        public List<SelectListItem> Distincts { get; set; }

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

            Cities = new List<SelectListItem>() { new SelectListItem() { Text = "Киев", Value = "1" },
                new SelectListItem() { Text = "Кременчуг", Value = "2" } };

            Distincts = new List<SelectListItem>() {
                new SelectListItem() { Text = "Дарницкий", Value = "1" },
                new SelectListItem() { Text = "Петровка", Value = "2" } 
            };
        }
    }
}