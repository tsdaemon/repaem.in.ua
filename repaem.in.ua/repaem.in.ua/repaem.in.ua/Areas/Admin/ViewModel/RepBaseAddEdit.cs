using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
    public class RepBaseAddEdit
    {
        public int Id { get; set; }

        [Display (Name="Название базы")]
        public string Name { get; set; }

        [Display(Name = "Город")]
        public Dictionary City { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public string Coordinates { get; set; }

        public GoogleMap Map { get; set; }

        public List<String> Photos { get; set; }

        public RepBaseAddEdit()
        {
            Map = new GoogleMap();
            Map.EditMode = true;
            Photos = new List<string>();
            City = new Dictionary("City");
        }
    }
}