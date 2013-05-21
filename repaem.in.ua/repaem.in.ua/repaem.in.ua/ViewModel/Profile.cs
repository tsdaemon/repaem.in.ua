using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Профіль користувача
    /// </summary>
    public class Profile
    {
        public int Id { get; set; }

        [Display(Name = "Имя"), MaxLength(128, ErrorMessage = "Слишком длинное имя!")]
        public string Name { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Подтверди пароль"), Compare("Password", ErrorMessage="Пароли не совпадают!")]
        public string CPassword { get; set; }

        [Display(Name = "Название группы"), MaxLength(128, ErrorMessage="Слишком длинное название!")]
        public string BandName { get; set; }

        [Display(Name="Телефон")]
        [Required(ErrorMessage="Требуется номер телефона!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Введенный номер неправильный!")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Город")]
        public Dictionary City { get; set; }

        public bool IsInBlackList { get; set; }

        public Profile()
        {
            City = new Dictionary("City");
        }
    }
}