using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Класс словарей. Предназначен для отображения данных типа ид - текст. 
    /// </summary>
    public class Dictionary
    {
        public List<SelectListItem> Items { get; set; }

        public int Value { get; set; }

        public string Display { get { if (Items != null) { var item = Items.Find((s) => s.Value == Value.ToString()); if (item != null) return item.Text; else return ""; } else return ""; } }

        public override string ToString()
        {
            return Display;
        }

        public Dictionary()
        {
            Items = new List<SelectListItem>();
        }
    }
}