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

        public string Display { get { if (Items != null||Items.Count <= Value) return Items[Value].Text; else return ""; } }

        public override string ToString()
        {
            return Display;
        }

        public Dictionary(string tableName)
        {
            if ((Items = HttpContext.Current.Cache.Get(tableName) as List<SelectListItem>) == null)
            {
                var db = new Database();
                Items = db.GetDictionary(tableName);
                HttpContext.Current.Cache.Insert(tableName, Items);
            }
        }
    }
}