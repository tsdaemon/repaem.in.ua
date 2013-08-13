using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using aspdev.repaem.Models;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Модель реєстрації
    /// </summary>
    public class Register
    {
        [Display(Name = "Номер мобильного",Description="Номер вашего телефона необходим для подтверждения заказа"), DataType(DataType.PhoneNumber), Required(ErrorMessage="Введите номер телефона!")]
        [RegularExpression("\\+[0-9]{11,14}$", ErrorMessage = "Введите телефон в международном формате +38xxxyyyyyyy")]
        public string Phone { get; set; }

        [Display(Name = "E-mail"), DataType(DataType.EmailAddress), Required(ErrorMessage = "Введите e-mail!")]
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage="Неправильный формат адреса!")]
        public string Email { get; set; }

        [Display(Name = "Пароль"), DataType(DataType.Password), Required(ErrorMessage = "Введите пароль!")]
        public string Password { get; set; }
				[Display(Name = "Повторите пароль"), DataType(DataType.Password), System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают!"), Required(ErrorMessage = "Введите подтвержение пароля!")]
        public string Password2 { get; set; }

				[Display(Name = "Кто вы")]
		    public string Role { get; set; }
				public List<SelectListItem> Roles { get; set; }
					
				[Display(Name = "Город")]
        public Dictionary City { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Капча"), Required(ErrorMessage = "Введите капчу!")]
        public Capcha Capcha { get; set; }

        public Register()
        {
          City = new Dictionary();
          Capcha = new Capcha();
	        Roles = new List<SelectListItem>()
		        {
			        new SelectListItem() {Value = UserRole.Musician.ToString(), Text = "Музыкант"},
			        new SelectListItem() {Value = UserRole.Manager.ToString(), Text = "Хозяин базы"}
		        };
        }
    }
}