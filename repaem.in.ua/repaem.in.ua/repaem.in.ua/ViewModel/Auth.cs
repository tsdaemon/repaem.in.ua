using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел аутетифікації
    /// </summary>
    public class Auth
    {
        //У нас логин может быть по этим полям
        [Display(Name = "Email\\телефон"), Required(ErrorMessage = "Введите логин!")]
        public string Login { get; set; }
        [Display(Name = "Пароль"), DataType(DataType.Password), Required(ErrorMessage = "Введите пароль!")]
        public string Password { get; set; }
        //количество попыток входа
        public int Count { get; set; }
    }
}