using System.Web.Security;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace aspdev.repaem.Security
{
	public class RepaemUserService
	{
		private readonly Database _db;
		private User user;
		private bool? _unpaidBill;
		private IMessagesProvider _msg;

		public RepaemUserService(Database db, IMessagesProvider msg, ISession ss)
		{
			_db = db;
			_msg = msg;
		}

		public bool ChangePassword(string login, string oldPassw, string newPassw)
		{
			var user = _db.SearchUser(login);
			if (GenerateMd5(oldPassw) == user.Password)
			{
				user.Password = GenerateMd5(newPassw);
				return true;
			}
			return false;
		}

		public bool ValidateUser(string login, string passw)
		{
			var user = _db.SearchUser(login);
			return GenerateMd5(passw) == user.Password;
		}

		public bool UserIsInRole(string role)
		{
			var user = CurrentUser;
			return user.Role.IndexOf(role, StringComparison.InvariantCultureIgnoreCase) > -1;
		}

		public bool UserIsInRole(string login, string role)
		{
			var user = _db.SearchUser(login);
			return user.Role.IndexOf(role, StringComparison.InvariantCultureIgnoreCase) > -1;
		}

		public bool Login(string login, string passw)
		{
			var user = _db.SearchUser(login);
			if (user != null && GenerateMd5(passw) == user.Password)
			{
				CurrentUser = user;
				return true;
			}
			else return false;
		}

		public void Logout()
		{
			FormsAuthentication.SignOut();
			CurrentUser = null;
		}

		public User CurrentUser
		{
			get 
			{
				return user;
			}
			private set { 
				user = value;
				FormsAuthentication.SetAuthCookie(user.Email, true);
			}
		}

		public bool? HaveUnpaidBill
		{
			get
			{
				if (CurrentUser == null)
					return null;

				return _unpaidBill ?? (_unpaidBill = _db.CheckUserBills(CurrentUser.Id));
			}
		}

		public User CreateUser(Register r)
		{
			var user = new User()
				{
					CityId = r.CityId,
					Email = r.Email,
					Name = r.Name,
					Password = GenerateMd5(r.Password),
					PhoneChecked = false,
					PhoneNumber = r.Phone,
					Role = r.Role
				};
			_db.CreateUser(user);

			_msg.SendMessage("Здраствуйте!", String.Format(@"Благодарим за регистрацию, {0}! 
Используйте для входа на сайт http://repaem.in.ua ваш телефон {1} или почтовый адрес {2}. 
Ваш пароль {3}.
С уважением, команда repaem.in.ua.", user.Name, user.PhoneNumber, user.Email, r.Password), null, new string[] { user.Email });

			CurrentUser = user;

			return user;
		}

		public void SaveProfile(Profile p)
		{
			User u = CurrentUser;
			if (u == null)
				throw new Exception("User is null!");

			u.BandName = p.BandName;
			if (!String.IsNullOrEmpty(p.Password)) //Инициализируем смену пароля, если введен
			{
				u.Password = GenerateMd5(p.Password);
			}
			u.PhoneChecked = u.PhoneNumber == p.PhoneNumber; //Снова проверять номер, если сменит
			u.PhoneNumber = p.PhoneNumber;
			u.Name = p.Name;
			u.CityId = p.CityId;
			u.Email = p.Email;

			_db.SaveUser(u);
		}

		public bool CheckEmailExist(string email)
		{
			return _db.CheckUserEmailExist(email);
		}

		public bool CheckPhoneExist(string phone)
		{
			return _db.CheckUserPhoneExist(phone);
		}

		public void SetCodeChecked()
		{
			if (CurrentUser == null) return;

			CurrentUser.PhoneChecked = true;
			_db.SaveUser(CurrentUser);
		}

		private static Guid GenerateMd5(string pass)
		{
			var md5 = MD5.Create();
			var inputBytes = Encoding.UTF8.GetBytes(pass);
			var hash = md5.ComputeHash(inputBytes);

			var g = new Guid(hash);
			return g;
		}

		internal User GetUser(string username)
		{
			var u = _db.SearchUser(username);
			if(u == null)
				throw new HttpException("Нет такого пользователя", 400);

			return u;
		}

		internal void SetUser(string username)
		{
			var u = _db.SearchUser(username);
			if (u == null)
				throw new HttpException("Нет такого пользователя", 400);

			user = u;
		}
	}
}
