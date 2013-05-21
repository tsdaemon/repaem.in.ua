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
    public class Feedback
    {
        [Display(Name="Ваше имя"), Required(ErrorMessage="Невежливо ругать других анонимно!")]
        public string Name { get; set; }

        [Display(Name = "Ваша почта"), DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Неправильная почта!")]
        public string Email { get; set; }

        [Display(Name = "Ваш отзыв"), DataType(DataType.MultilineText), Required(ErrorMessage = "А отзыв?")]
        public string Text { get; set; }

        [Display(Name = "Капча"), Required(ErrorMessage = "Введите капчу!")]
        public Capcha Capcha { get; set; }

        public List<Feedback> Previous { get; set; }

        public Feedback()
        {
            //TODO: получить список отзывов, которые уже есть. Все будут выводиться на одной странице
            Previous = new List<Feedback>();
        }
    }
}