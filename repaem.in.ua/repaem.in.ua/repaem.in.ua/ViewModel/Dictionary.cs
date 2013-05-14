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
            
            //TODO: TO KCH якщо є в кеші - дістати, якщо нема - дістати з бази і покласти в кеш, назву таблиці дивитися по атрібуту
            //кеш - гугли класс для mvc Cache
        }

        public Dictionary(string tableName)
        {
            Items = new List<SelectListItem>();
            //TODO: TO KCH або діставати по тейблнейм тут
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