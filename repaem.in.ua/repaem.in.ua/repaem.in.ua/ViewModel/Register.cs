using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Модель реєстрації
    /// </summary>
    public class Register
    {
        //TODO: запилить регулярку на телефон
        [Display(Name = "Номер мобильного",Description="Номер вашего телефона необходим для подтверждения заказа"), DataType(DataType.PhoneNumber), Required(ErrorMessage="Введите номер телефона!")]
        public string Phone { get; set; }
        [Display(Name = "E-mail"), DataType(DataType.EmailAddress), Required(ErrorMessage = "Введите e-mail!")]
        public string Email { get; set; }

        [Display(Name = "Пароль"), DataType(DataType.Password), Required(ErrorMessage = "Введите пароль!")]
        public string Password { get; set; }
        [Display(Name = "Повторите пароль"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Пароли не совпадают!"), Required(ErrorMessage = "Введите подтвержение пароля!")]
        public string Password2 { get; set; }

        [Display(Name = "Город")]
        public Dictionary City { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Капча"), Required(ErrorMessage = "Введите капчу!")]
        public Capcha Capcha { get; set; }

        public Register()
        {
            City = new Dictionary("City");
            Capcha = new Capcha();
        }
    }
}