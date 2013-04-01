using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
    //TODO:Нужно продумать механизм, что бы данные о фильтре сохранялись в сессии
    public class RepBaseFilter
    {
        [Display(Name="Название базы")]
        public string Name { get; set; }

        [Display(Name="Город")]
        public int City { get; set; }

        public string CityName { get { return Cities.Find((c) => c.Value == City.ToString()).Text; } }

        public List<SelectListItem> Cities { get; set; }

        //Вообще, есть идея написать отдельную обработку словарей.
        //1. Автоматическая выборка из базы по названию таблицы
        //2. Свой хелпер (можно сделать что-то в стиле jquery ui)
        //3. Автоматом сохранение и загрузка из кэша

        [Display(Name = "Район")]
        public int Distinct { get; set; }

        public string DistinctName { get { return Distincts.Find((c) => c.Value == Distinct.ToString()).Text; } }

        public List<SelectListItem> Distincts { get; set; }

        [Display(Name = "Дата"), DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Время"), Range(0, 24)]
        public TimeRange Time { get; set; }

        [Display(Name = "Стоимость"), Range(0, 100)]
        public Range Price { get; set; }

        public DisplayType DisplayTpe { get; set; }

        public RepBaseFilter()
        {
            //TODO: TO(KCH) get dictionaries from cache
            Cities = new List<SelectListItem>() { new SelectListItem() { Text = "Киев", Value = "1" },
                    new SelectListItem() { Text = "Кременчуг", Value = "2" } };

            Distincts = new List<SelectListItem>() {
                    new SelectListItem() { Text = "Дарницкий", Value = "1" },
                    new SelectListItem() { Text = "Петровка", Value = "2" } 
                };

            Date = DateTime.Today;

            Time = new TimeRange(4, 20);
            Price = new Range(25, 75);
        }

        public enum DisplayType { inline, square }
    }
}