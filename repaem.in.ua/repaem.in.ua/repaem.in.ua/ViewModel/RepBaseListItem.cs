using System.ComponentModel.DataAnnotations;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   ВьюМодел відображення бази у списку
	/// </summary>
	public class RepBaseListItem
	{
		public int Id { get; set; }

		[Display(Name = "Название")]
		public string Name { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }

		[Display(Name = "Рейтинг")]
		public string Rating { get; set; }

		public int RatingCount { get; set; }

		/// <summary>
		///   Посилання на лого бази
		/// </summary>
		[Display(Name = "Логотип")]
		public string ImageSrc { get; set; }

		[Display(Name = "Адрес")]
		public string Address { get; set; }
	}
}