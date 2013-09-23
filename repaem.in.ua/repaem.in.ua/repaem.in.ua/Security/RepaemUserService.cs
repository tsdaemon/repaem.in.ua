﻿using aspdev.repaem.Infrastructure.Exceptions;
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
		User CurrentUser { get; }
		bool? HaveUnpaidBill { get; }
		bool ChangePassword(string login, string oldPassw, string newPassw);
		bool ValidateUser(string login, string passw);
		bool UserIsInRole(string role);
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
		private readonly ILogger _lg;
		private readonly ISession _ss;
		private bool? _unpaidBill;
		private const string cookieKey = "ASP.NET_SessionId";

		public RepaemUserService(IDatabase db, ILogger lg, IEmailSender email, ISession ss)
		{
			_lg = lg;
			_db = db;
			_email = email;
			_ss = ss;
		}

		public bool ChangePassword(string login, string oldPassw, string newPassw)
		{
			var user = _db.GetUser(login);
			if (GenerateMd5(oldPassw) == user.Password)
			{
				user.Password = GenerateMd5(newPassw);
				return true;
			}
			return false;
		}

		public bool ValidateUser(string login, string passw)
		{
			var user = _db.GetUser(login);
			return GenerateMd5(passw) == user.Password;
		}

		public bool UserIsInRole(string role)
		{
			var user = CurrentUser;
			return user.Role.IndexOf(role, StringComparison.InvariantCultureIgnoreCase) > -1;
		}

		public bool UserIsInRole(string login, string role)
		{
			var user = _db.GetUser(login);
			return user.Role.IndexOf(role, StringComparison.InvariantCultureIgnoreCase) > -1;
		}

		public bool Login(string login, string passw)
		{
			var user = _db.GetUser(login);
			if (user != null && GenerateMd5(passw) == user.Password)
			{
				CurrentUser = user;
				return true;
			}
			else return false;
		}

		public void Logout()
		{
			CurrentUser = null;
		}

		public User CurrentUser
		{
			get
			{
				User u = _ss.User;
				if (u == null)
				{
					var coo = HttpContext.Current.Request.Cookies[cookieKey];
					if (coo != null)
					{
						u = HttpContext.Current.Cache[coo.Value] as User;
					}
				}

				return u;
			}
			private set { 
				_ss.User = value;
				var coo = HttpContext.Current.Request.Cookies[cookieKey];
				if (coo != null)
				{
					if (value == null) 
						HttpContext.Current.Cache.Remove(coo.Value);
					else 
						HttpContext.Current.Cache[coo.Value] = value;
				}
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
	}
}
