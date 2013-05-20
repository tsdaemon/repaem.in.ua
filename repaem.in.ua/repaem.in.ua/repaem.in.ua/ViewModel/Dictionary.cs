using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
    public class Dictionary
    {
        public List<SelectListItem> Items { get; set; }

        public int Value { get; set; }

        public string Display { get { if (Items != null||Items.Count <= Value) return Items[Value].Text; else return ""; } }

        public override string ToString()
        {
            return Display;
        }

        public Dictionary()
        {
            Items = new List<SelectListItem>();
        }

        public Dictionary(string tableName)
        {
            Items = new List<SelectListItem>();
            //TODO: TO KCH або діставати по тейблнейм тут
            //TODO: TO KCH діставати словник. І записувати дані в Items. Якщо tableName = City, дістати список City
            //кеш - гугли класс для mvc Cache
        }
    }

    /// <summary>
    /// Назва таблиці
    /// </summary>
    public class DictionaryAttribute
    {
        public string TableName { get; set; }
    }
}