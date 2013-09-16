using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел для комнаты репетиционной базы
    /// </summary>
    public class RepBaseRoom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Фотографії кімнати
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// Календар репетицій
        /// </summary>
        public Calendar Calendar { get; set; }

        //используется либо это, если есть сложная цена
        public List<Price> Prices { get; set; }

        //либо это
        public int Price { get; set; }

        public RepBaseRoom()
        {
            Images = new List<Image>();
            Calendar = new Calendar();
						Prices = new List<Price>();
        }
    }
}