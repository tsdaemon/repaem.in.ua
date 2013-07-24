using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Відгук про сайт
    /// </summary>
    public class Comment
    {
        int repId;
        public int RepBaseId { get { return repId; } set { repId = value; } }
        public string RepBaseName { get; set; }

        public int? UserId { get; set; }

        [Display(Name="Ваше имя"), Required(ErrorMessage="Невежливо ругать других анонимно!")]
        public string Name { get; set; }

        [Display(Name = "Ваша почта"), DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Неправильная почта!")]
        public string Email { get; set; }

        [Display(Name = "Ваш отзыв"), DataType(DataType.MultilineText), Required(ErrorMessage = "А отзыв?")]
        public string Text { get; set; }

        [Display(Name = "Капча"), Required(ErrorMessage = "Введите капчу!")]
        public Capcha Capcha { get; set; }

        [Display(Name = "Ваша оценка")]
        public double Rating { get; set; }

        public Comment()
        {

        }
    }
}