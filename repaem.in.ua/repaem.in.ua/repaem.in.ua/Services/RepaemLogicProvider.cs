﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OAuth2;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Models;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using aspdev.repaem.ViewModel.PageModel;
using System.Web.Mvc.Razor;
using Comment = aspdev.repaem.ViewModel.Comment;
using RepBase = aspdev.repaem.ViewModel.RepBase;
using Repetition = aspdev.repaem.ViewModel.Repetition;

namespace aspdev.repaem.Services
{
	public class RepaemLogicProvider
	{
		private readonly Database _db;
		private readonly ISession _ss;
		private AuthorizationRoot _auth;
		private ISmsSender _sms;

		public RepaemLogicProvider(Database db, ISession ss, RepaemUserService us, AuthorizationRoot authorizationRoot, ISmsSender sms)
		{
			_db = db;
			_ss = ss;
			UserData = us;
			_auth = authorizationRoot;
			_sms = sms;
		}

		public RepaemUserService UserData { get; private set; }

		public List<SelectListItem> GetDictionaryValues(string name)
		{
			List<SelectListItem> ls = _db.GetDictionary(name);
			ls.Insert(0, new SelectListItem {Text = "", Value = "0"});
			return ls;
		}

		public List<SelectListItem> GetDictionaryValues(string name, int fKey)
		{
			List<SelectListItem> ls = _db.GetDictionary(name, fKey);
			ls.Insert(0, new SelectListItem {Text = "", Value = "0"});
			return ls;
		}

		public Register GetRegisterModel()
		{
			var r = new Register();
			r.Cities = GetDictionaryValues("Cities");
			return r;
		}

		public RepBaseFilter GetFilter()
		{
			var f = new RepBaseFilter();
			f.Cities = GetDictionaryValues("Cities");
			return f;
		}

		public Profile GetProfile()
		{
			Profile p = _db.GetProfile(UserData.CurrentUser.Id);
			p.Cities = GetDictionaryValues("Cities");
			return p;
		}

		public RepBaseList GetAllRepBasesList()
		{
			var l = new RepBaseList();
			l.Filter = GetFilter();
			l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
			l.Map = new GoogleMap();
			l.Map.Coordinates = _db.GetAllBasesCoordinates();
			l.RepBases = _db.GetAllBases();
			return l;
		}

		public RepBaseFilter LoadFilterDictionaries(RepBaseFilter f)
		{
			if (f.CityId != 0)
			{
				f.Cities = GetDictionaryValues("Cities");
			}
			return f;
		}

		public RepBaseList GetRepBasesByFilter(RepBaseFilter f)
		{
			f = LoadFilterDictionaries(f);
			var l = new RepBaseList();
			l.Filter = f;
			l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
			l.RepBases = _db.GetBasesByFilter(f);
			l.Map.Coordinates = _db.GetBasesCoordinatesByList(l.RepBases);

			_ss.BookDate = f.Date;
			_ss.BookTime = f.Time;

			return l;
		}

		public bool TryDemoData()
		{
			try
			{
				_db.DeleteDemoData();
				_db.CreateDemoData();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public RepBaseBook GetRepBaseBook(int id)
		{
			var b = new RepBaseBook
				{
					Date = _ss.BookDate.HasValue ? _ss.BookDate.Value : DateTime.Today,
					Time = (_ss.BookTime ?? new TimeRange(12, 18)),
					RepBaseName = _db.GetBaseName(id),
					RepBaseId = id,
					Rooms = GetDictionaryValues("Rooms", id)
				};
			b.Rooms.RemoveAt(0); //что бы пустого не было

			return b;
		}

		public RepBaseBook GetRepBaseBook(int id, DateTime datetime, int roomid)
		{
			var b = new RepBaseBook
				{
					Date = datetime,
					Time = new TimeRange(datetime.Hour, datetime.Hour + 2),
					RepBaseName = _db.GetBaseName(id),
					RepBaseId = id,
					Rooms = GetDictionaryValues("Rooms", id)
				};
			b.Rooms.RemoveAt(0); //что бы пустого не было
			b.Rooms.ForEach((r) =>
				{
					if (r.Value == roomid.ToString())
					{
						r.Selected = true;
					}
				}); //Выбираем именно эту комнату

			return b;
		}

		public void UpdateRepBaseBook(RepBaseBook rb)
		{
			rb.Rooms = GetDictionaryValues("Rooms", rb.RepBaseId);
			rb.RepBaseName = GetRepBaseName(rb.RepBaseId);
		}

		public List<RepbaseInfo> GetAllBasesCoordinates()
		{
			return _db.GetAllBasesCoordinates();
		}

		public HomeIndexModel GetHomeIndexModel()
		{
			var m = new HomeIndexModel();
			m.NewBases = _db.GetNewBases().ToList();
			m.Map = new GoogleMap {Coordinates = _db.GetAllBasesCoordinates()};
			m.Filter = GetFilter();
			m.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;
			return m;
		}

		public Profile GetUserProfile()
		{
			if (UserData.CurrentUser != null)
			{
				Profile pf = _db.GetProfile(UserData.CurrentUser.Id);
				pf.Cities = GetDictionaryValues("Cities");
				return pf;
			}
			else throw new Exception("User is null!");
		}

		public void SaveProfile(Profile p)
		{
			UserData.SaveProfile(p);
			p.Cities = GetDictionaryValues("Cities");
		}

		public List<Repetition> GetRepetitions()
		{
			//Только новые репетиции
			IEnumerable<Repetition> reps = from r in _db.GetRepetitions(UserData.CurrentUser.Id)
			                               where r.Date >= DateTime.Today
			                               select r;
			return reps.ToList();
		}

		public void SaveComment(Comment c)
		{
			var c1 = new Models.Data.Comment
				{
					Host = HttpContext.Current.Request.UserHostAddress,
					Email = c.Email,
					Name = c.Name,
					Rating = c.Rating,
					RepBaseId = c.RepBaseId,
					Text = c.Text,
					Date = DateTime.Now
				};

			if (UserData.CurrentUser != null)
			{
				c1.UserId = UserData.CurrentUser.Id;
			}

			_db.SaveComment(c1);
		}

		public AuthOrRegister GetAuthOrRegister()
		{
			var au = new AuthOrRegister();
			au.Register.Cities = GetDictionaryValues("Cities");
			return au;
		}

		public void SaveBook(RepBaseBook rb)
		{
			//don't play with time...
			DateTime begin = rb.Date.AddHours(rb.Time.Begin);
			if (DateTime.Now >= begin)
				throw new RepaemItIsPastException("Не пытайтесь обмануть время");

			if (!_db.CheckRepetitionTime(rb.Time, rb.Date, rb.RoomId))
				throw new RepaemTimeIsBusyException("Это время уже занято! Попробуйте другое!");

			if (!UserData.CurrentUser.PhoneChecked)
				throw new RepaemPhoneNotCheckedException("Музыкант с непроверенным номером не может заказывать репетици");

			var r = new Models.Data.Repetition
				{
					Comment = rb.Comment,
					MusicianId = UserData.CurrentUser.Id,
					RepBaseId = rb.RepBaseId,
					RoomId = rb.RoomId,
					Status = (int) Status.ordered,
					TimeStart = rb.Time.Begin,
					TimeEnd = rb.Time.End,
					Date = rb.Date,
					Sum = _db.GetRepetitionSum(rb)
				};
			_db.AddRepetition(r);

			//Сообщение о заказе
			rb.Rooms = _db.GetDictionary("Rooms", rb.RepBaseId);
			User master = _db.GetRepBaseMaster(rb.RepBaseId);
			User musician = UserData.CurrentUser;

			//TODO: отправить уведомление
			//string msg =
			//	String.Format("Заказана репетиция: репетиционная база {0}, комната {1}, {6} {2}.00-{3}.00, музыкант {4} {5}",
			//								rb.RepBaseName, rb.Rooms.Find((r1)=>r1.Selected).Text, rb.Time.Begin, rb.Time.End, musician.Name, musician.PhoneNumber,
			//								rb.Date);
			//_msg.SendMessage(msg, new[] {master.PhoneNumber}, new[] {master.Email, musician.Email});
		}

		public RepBase GetRepBase(int id)
		{
			RepBase info = _db.GetRepBase(id);
			foreach (var r in info.Rooms)
			{
				r.Calendar.AddUrl = UrlHelper.GenerateUrl("Default", "BookRoom", "RepBase", null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, true);
			}
			return info;
		}

		public string GetRepBaseName(int id)
		{
			return _db.GetBaseName(id);
		}

		public void CancelRepetition(int id)
		{
			//проверяем, не чужая ли это репа
			var rep = _db.GetRepetitionInfo(id);
			if(rep.MusicianId != UserData.CurrentUser.Id)
				throw new RepaemAccessDeniedException("Вы не можете отменить чужую репетицию!");

			var manager = _db.GetRepBaseMaster(rep.RepBaseId);
			var room = _db.GetOne<Room>(rep.RoomId);
			var repBase = _db.GetOne<Models.Data.RepBase>(rep.RepBaseId);

			string msg;

			switch (rep.Status)
			{
				case (int)Status.approoved:
				case (int)Status.ordered:
					msg = String.Format("Репетиция на базе {0}, комната {1}, время {2}.00-{3}.00 {4} отменена",
					                    repBase.Name, room.Name, rep.TimeStart, rep.TimeEnd, rep.Date);
					_db.SetRepetitionStatus(id, Status.cancelled);
					break;
				default:
					throw new RepaemRepetitionWrongStatusException("Невозможно отменить репетицию c неверным статусом")
						{
							Status = (Status) rep.Status
						};
			}
			//TODO: отослать оповещение
		}

		public Comments GetRepBaseComments(int id)
		{
			var c = new Comments(_db.GetRepBaseComments(id))
				{
					RepBaseId = id,
					RepBaseName = _db.GetBaseName(id)
				};
			return c;
		}

		public bool CheckCanRate(int id)
		{
			return UserData.CurrentUser != null
				       ? _db.CheckCanCommentRepBase(id, UserData.CurrentUser.Id, HttpContext.Current.Request.UserHostAddress)
				       : _db.CheckCanCommentRepBase(id, HttpContext.Current.Request.UserHostAddress);
		}

		internal StringDictionary GetSocialLinks()
		{
			var dic = new StringDictionary();
			foreach (var au in _auth.Clients)
			{
				dic.Add(au.Name, au.GetLoginLinkUri());
			}
			return dic;
		}

		internal void SendCodeSms(string phoneNumber)
		{
			Random r = new Random(5);
			int code = r.Next(256742, 999999);
			_ss.Sms = code;

			_sms.SendSms(phoneNumber, String.Format("Ваш код подтверждения: {0}", code));
		}

		internal int GetRepBaseByRoom(int roomid)
		{
			return _db.GetRepBaseByRoom(roomid);
		}
	}
}