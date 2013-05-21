using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{ 
    /// <summary>
    /// ViewModel для отображения и редактирования фильтра репетиционных баз
    /// </summary>
    public class RepBaseFilter
    {
        [Display(Name="Название базы")]
        public string Name { get; set; }

        [Display(Name = "Город")]
        public Dictionary City { get; set; }

        [Display(Name = "Район")]
        public Dictionary Distinct { get; set; }

        [Display(Name = "Дата"), DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Время"), Range(0, 24)]
        public TimeRange Time { get; set; }

        [Display(Name = "Стоимость"), Range(0, 100)]
        public Range Price { get; set; }

        public DisplayType DisplayTpe { get; set; }

        public RepBaseFilter()
        {
            City = new Dictionary("City");
            City.Items.Add(new SelectListItem() { Text = "Киев", Value = "1" });
            City.Items.Add(new SelectListItem() { Text = "Кременчуг", Value = "2" });

            Distinct = new Dictionary("Distinct");
            Distinct.Items.Add(new SelectListItem() { Text = "Дарницкий", Value = "1" });

            Date = DateTime.Today;

            Time = new TimeRange(4, 20);
            Price = new Range(25, 75);
        }

        public enum DisplayType { inline, square }
    }
}