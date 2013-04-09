using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Register
    {
        //TODO: запилить регулярку на телефон
        [Display(Name = "Номер мобильного"), DataType(DataType.PhoneNumber), Required]
        public string Phone { get; set; }
        [Display(Name = "E-mail"), DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }

        [Display(Name = "Пароль"), DataType(DataType.Password), Required]
        public string Password { get; set; }
        [Display(Name = "Повторите пароль"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Пароли не совпадают!"), Required]
        public string Password2 { get; set; }

        [Display(Name = "Город")]
        public Dictionary City { get; set; }
        [Display(Name = "Имя"), Required]
        public string Name { get; set; }

        [Display(Name = "Капча"), Required]
        public Capcha Capcha { get; set; }

        public Register()
        {
            City = new Dictionary("City");
            Capcha = new Capcha();
        }
    }
}