using System;
using System.ComponentModel.DataAnnotations;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   Відгук про сайт
	/// </summary>
	public class Comment
	{
		public int Id { get; set; }

		public int RepBaseId { get; set; }

		[Display(ShortName = "База")]
		public string RepBaseName { get; set; }

		public int? UserId { get; set; }

		[Display(Name = "Ваше имя", ShortName = "Имя"), Required(ErrorMessage = "Невежливо ругать других анонимно!")]
		public string Name { get; set; }

		[Display(Name = "Ваша почта", ShortName = "E-mail"), DataType(DataType.EmailAddress)]
		[RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Неправильная почта!")]
		public string Email { get; set; }

		[Display(Name = "Ваш отзыв", ShortName = "Отзыв"), DataType(DataType.MultilineText), Required(ErrorMessage = "А отзыв?")]
		public string Text { get; set; }

		[Display(Name = "Капча"), Required(ErrorMessage = "Введите капчу!")]
		public Capcha Capcha { get; set; }

		public DateTime Date { get; set; }

		[Display(Name = "Ваша оценка", ShortName = "Оценка")]
		public double Rating { get; set; }
	}
}