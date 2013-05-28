using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
    public class RepBaseListItem
    {
        public int Id { get; set; }

        [Display(Name="Название базы")]
        public string Name { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Рейтинг")]
        public float Rating { get; set; }
    }
}