using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   Профіль користувача
	/// </summary>
	public class Profile
	{
		public int Id { get; set; }

		[Display(Name = "Имя"), MaxLength(128, ErrorMessage = "Слишком длинное имя!")]
		public string Name { get; set; }

		[Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Подтвердите пароль"), System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают!")]
		[DataType(DataType.Password)]
		public string CPassword { get; set; }

		[Display(Name = "Название группы"), MaxLength(128, ErrorMessage = "Слишком длинное название!")]
		public string BandName { get; set; }

		[Display(Name = "Телефон")]
		[Required(ErrorMessage = "Требуется номер телефона!")]
		[RegularExpression("\\+[0-9]{11,14}$", ErrorMessage = "Введите телефон в международном формате +38xxxyyyyyyy")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[Display(Name = "E-mail")]
		[DataType(DataType.EmailAddress)]
		[RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Неправильный формат адреса!")]
		public string Email { get; set; }

		[Display(Name = "Город")]
		public int CityId { get; set; }
		public List<SelectListItem> Cities { get; set; }

		public bool IsInBlackList { get; set; }

		public Profile()
		{
		}
	}
}