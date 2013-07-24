using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ViewModel для репетиционной базы, отображение на одной страницы
    /// </summary>
    public class RepBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display (Name="Город")]
        public string City { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        //[Display(Name = "Контакты")]
        //public string Contacts { get; set; }

        [Display(Name = "Описание базы")]
        public string Description { get; set; }

        /// <summary>
        /// Фото бази
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// Мапа з координатами бази
        /// </summary>
        public GoogleMap Map { get; set; }

        //TODO: comments

        /// <summary>
        /// Список кімнат
        /// </summary>
        [UIHint("RepBaseRoomList")]
        public List<RepBaseRoom> Rooms { get; set; }

        /// <summary>
        /// Рейтинг. Розраховується як середнє арифметичне всіх попередніх оцінок
        /// </summary>
        public double Rating { get; set; }

        public RepBase()
        {
            Images = new List<Image>();
            Map = new GoogleMap();
            Rooms = new List<RepBaseRoom>();
        }
    }
}