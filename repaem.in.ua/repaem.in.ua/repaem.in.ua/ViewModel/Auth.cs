using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Auth
    {
        //У нас логин может быть по этим полям
        [Display(Name="Email\\телефон")]
        public string Login { get; set; }
        [Display(Name = "Пароль"), DataType(DataType.Password)]
        public string Password { get; set; }
        //количество попыток входа
        public int Count { get; set; }
    }
}