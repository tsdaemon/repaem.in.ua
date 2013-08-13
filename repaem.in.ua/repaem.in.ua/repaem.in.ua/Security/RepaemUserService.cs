using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace aspdev.repaem.Security
{
	public interface IUserService
	{
		//TODO: восстановить пароль
		//TODO: Объединить с обычной сессией
		User CurrentUser { get; }
		bool ChangePassword(string login, string oldPassw, string newPassw);
		bool ValidateUser(string login, string passw);
		bool UserIsInRole(string login, string role);
		bool CheckEmailExist(string email);
		bool CheckPhoneExist(string phone);
		bool Login(string login, string passw);
		void Logout();
		User CreateUser(Register r);
		void SaveProfile(Profile p);

		void SetCodeChecked();
	}

	public class RepaemUserService : IUserService
	{
		private readonly IDatabase _db;
		private readonly IEmailSender _email;
		private ILogger _lg;

		public RepaemUserService(IDatabase db, ILogger lg, IEmailSender email)
		{
			_lg = lg;
			_db = db;
			_email = email;
		}

		public bool ChangePassword(string login, string oldPassw, string newPassw)
		{
			var user = _db.GetUser(login);
			if (GenerateMD5(oldPassw) == user.Password)
			{
				user.Password = GenerateMD5(newPassw);
				return true;
			}
			return false;
		}

		public bool ValidateUser(string login, string passw)
		{
			var user = _db.GetUser(login);
			return GenerateMD5(passw) == user.Password;
		}

		public bool UserIsInRole(string login, string role)
		{
			var user = _db.GetUser(login);
			return user.Role.IndexOf(role, StringComparison.InvariantCultureIgnoreCase) > -1;
		}

		public bool Login(string login, string passw)
		{
			var user = _db.GetUser(login);
			if (user != null && GenerateMD5(passw) == user.Password)
			{
				CurrentUser = user;
				return true;
			}
			else return false;
		}

		public void Logout()
		{
			HttpCookie c = HttpContext.Current.Request.Cookies.Get("user_session");
			if (c != null)
			{
				HttpContext.Current.Cache.Remove(c.Value);
				HttpContext.Current.Response.Cookies.Remove("user_session");
			}
		}

		public User CurrentUser
		{
			get
			{
				HttpCookie c = HttpContext.Current.Request.Cookies["user_session"];
				if (c != null)
				{
					var u = HttpContext.Current.Cache[c.Value] as User;
					return u;
				}
				else return null;
			}

			private set
			{
				Guid gg = Guid.NewGuid();
				HttpContext.Current.Response.Cookies.Add(new HttpCookie("user_session", gg.ToString()));
				HttpContext.Current.Cache.Insert(gg.ToString(), value, null, DateTime.MaxValue, new TimeSpan(1, 0, 0, 0));
			}
		}

		public User CreateUser(Register r)
		{
			var user = new User()
				{
					CityId = r.City.Value,
					Email = r.Email,
					Name = r.Name,
					Password = GenerateMD5(r.Password),
					PhoneChecked = false,
					PhoneNumber = r.Phone,
					Role = r.Role
				};
			_db.CreateUser(user);
			_email.SendRegisteredMail(user);
			_lg.Trace(string.Format("Зарегистрировался пользователь {0}", user.Name));
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
				u.Password = GenerateMD5(p.Password);
			}
			u.PhoneChecked = u.PhoneNumber == p.PhoneNumber; //Снова проверять номер, если сменит
			u.PhoneNumber = p.PhoneNumber;
			u.Name = p.Name;
			u.CityId = p.City.Value;
			u.Email = p.Email;

			_db.SaveUser(u);
		}

		public bool CheckEmailExist(string Email)
		{
			return _db.CheckUserEmailExist(Email);
		}

		public bool CheckPhoneExist(string Phone)
		{
			return _db.CheckUserPhoneExist(Phone);
		}

		public void SetCodeChecked()
		{
			if (CurrentUser != null)
			{
				CurrentUser.PhoneChecked = true;
				_db.SaveUser(CurrentUser);
			}
		}

		private Guid GenerateMD5(string pass)
		{
			MD5 md5 = MD5.Create();
			byte[] inputBytes = Encoding.UTF8.GetBytes(pass);
			byte[] hash = md5.ComputeHash(inputBytes);

			Guid g = new Guid(hash);
			return g;
		}
	}
}