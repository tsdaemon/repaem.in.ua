using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel.PageModel;
using Comment = aspdev.repaem.Models.Data.Comment;
using RepBase = aspdev.repaem.ViewModel.RepBase;
using Repetition = aspdev.repaem.Models.Data.Repetition;

namespace aspdev.repaem.Services
{
	public interface IRepaemLogicProvider
	{
		IUserService UserData { get; }

		List<RepbaseInfo> GetAllBasesCoordinates();

		RepBaseList GetAllRepBasesList();

		RepBaseList GetRepBasesByFilter(RepBaseFilter f);

		Register GetRegisterModel();

		RepBaseFilter GetFilter();

		Profile GetProfile();

		List<SelectListItem> GetDictionaryValues(string name);

		List<SelectListItem> GetDictionaryValues(string name, int fKey);

		RepBaseFilter LoadFilterDictionaries(RepBaseFilter f);

		RepBaseBook GetRepBaseBook(int id);

		RepBaseBook GetRepBaseBook(int id, DateTime dateTime, int p, int roomid);

		bool TryDemoData();

		HomeIndexModel GetHomeIndexModel();

		Profile GetUserProfile();

		void SaveProfile(Profile p);

		List<ViewModel.Repetition> GetRepetitions();

		void SaveComment(ViewModel.Comment c);

		AuthOrRegister GetAuthOrRegister();

		bool SaveBook(RepBaseBook rb);

		RepBase GetRepBase(int id);

		string GetRepBaseName(int id);

		void CancelRepetition(int id);

		void UpdateRepBaseBook(RepBaseBook rb);

		/// <summary>
		/// Перевірити, чи може цей користувач оцінювати базу
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>Первірка по користувачу та по IP</remarks>
		/// <returns></returns>
		bool CheckCanRate(int id);

		Comments GetRepBaseComments(int id);
	}

	public class RepaemLogicProvider : IRepaemLogicProvider
	{
		private readonly IDatabase db;
		private readonly IEmailSender email;
		private readonly ISmsSender sms;
		private readonly ISession ss;

		public RepaemLogicProvider(IDatabase _db, ISession _ss, IUserService _us, ISmsSender _sms, IEmailSender _email)
		{
			db = _db;
			ss = _ss;
			sms = _sms;
			email = _email;
			UserData = _us;
		}

		public IUserService UserData { get; private set; }

		public List<SelectListItem> GetDictionaryValues(string name)
		{
			var ls = db.GetDictionary(name);
			ls.Insert(0, new SelectListItem() {Text = "", Value = "0"});
			return ls;
		}

		public List<SelectListItem> GetDictionaryValues(string name, int fKey)
		{
			var ls = db.GetDictionary(name, fKey);
			ls.Insert(0, new SelectListItem() {Text = "", Value = "0"});
			return ls;
		}

		public Register GetRegisterModel()
		{
			var r = new Register();
			r.City.Items = GetDictionaryValues("Cities");
			return r;
		}

		public RepBaseFilter GetFilter()
		{
			var f = new RepBaseFilter();
			f.City.Items = GetDictionaryValues("Cities");
			f.Distinct.Items.Add(new SelectListItem() {Text = "", Value = "0"});
			return f;
		}

		public Profile GetProfile()
		{
			var p = db.GetProfile(UserData.CurrentUser.Id);
			p.City.Items = GetDictionaryValues("Cities");
			return p;
		}

		public RepBaseList GetAllRepBasesList()
		{
			RepBaseList l = new RepBaseList();
			l.Filter = GetFilter();
			l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
			l.Map = new GoogleMap();
			l.Map.Coordinates = db.GetAllBasesCoordinates();
			l.RepBases = db.GetAllBases();
			return l;
		}

		public RepBaseFilter LoadFilterDictionaries(RepBaseFilter f)
		{
			if (f.City.Value != 0)
			{
				f.City.Items = GetDictionaryValues("Cities");
				if (f.Distinct.Value != 0)
				{
					f.Distinct.Items = GetDictionaryValues("Distincts", f.City.Value);
				}
			}
			return f;
		}

		public RepBaseList GetRepBasesByFilter(RepBaseFilter f)
		{
			f = LoadFilterDictionaries(f);
			var l = new RepBaseList();
			l.Filter = f;
			l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
			l.RepBases = db.GetBasesByFilter(f);
			l.Map.Coordinates = db.GetBasesCoordinatesByList(l.RepBases);

			ss.BookDate = f.Date;
			ss.BookTime = f.Time;

			return l;
		}

		public bool TryDemoData()
		{
			try
			{
				db.DeleteDemoData();
				db.CreateDemoData();
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
					Date = ss.BookDate.HasValue ? ss.BookDate.Value : DateTime.Today,
					Time = (ss.BookTime ?? new TimeRange(12, 18)),
					RepBaseName = db.GetBaseName(id),
					RepBaseId = id,
					Room = {Items = GetDictionaryValues("Rooms", id)}
				};
			b.Room.Items.RemoveAt(0); //что бы пустого не было

			return b;
		}

		public RepBaseBook GetRepBaseBook(int id, DateTime date, int time, int roomid)
		{
			var b = new RepBaseBook
			{
				Date = date,
				Time = new TimeRange(time, time + 2),
				RepBaseName = db.GetBaseName(id),
				RepBaseId = id,
				Room = { Items = GetDictionaryValues("Rooms", id) }
			};
			b.Room.Items.RemoveAt(0); //что бы пустого не было
			b.Room.Items.ForEach((r) => {
				                            if (r.Value == roomid.ToString())
				                            {
					                            r.Selected = true;
				                            }
			}); //Выбираем именно эту комнату

			return b;
		}

		public void UpdateRepBaseBook(RepBaseBook rb)
		{
			rb.Room.Items = GetDictionaryValues("Rooms", rb.RepBaseId);
			rb.RepBaseName = GetRepBaseName(rb.RepBaseId);
		}

		public List<RepbaseInfo> GetAllBasesCoordinates()
		{
			return db.GetAllBasesCoordinates();
		}

		public HomeIndexModel GetHomeIndexModel()
		{
			HomeIndexModel m = new HomeIndexModel();
			m.NewBases = db.GetNewBases().ToList();
			m.Map = new GoogleMap() {Coordinates = db.GetAllBasesCoordinates()};
			m.Filter = GetFilter();
			m.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;
			return m;
		}

		public Profile GetUserProfile()
		{
			if (UserData.CurrentUser != null)
			{
				var pf = db.GetProfile(UserData.CurrentUser.Id);
				pf.City.Items = GetDictionaryValues("Cities");
				return pf;
			}
			else throw new Exception("User is null!");
		}

		public void SaveProfile(Profile p)
		{
			UserData.SaveProfile(p);
			p.City.Items = GetDictionaryValues("Cities");
		}

		public List<ViewModel.Repetition> GetRepetitions()
		{
			//Только новые репетиции
			var reps = from r in db.GetRepetitions(UserData.CurrentUser.Id)
			           where r.Date >= DateTime.Today
			           select r;
			return reps.ToList();
		}

		public void SaveComment(ViewModel.Comment c)
		{
			var c1 = new Comment
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

			db.SaveComment(c1);
		}

		public AuthOrRegister GetAuthOrRegister()
		{
			var au = new AuthOrRegister();
			au.Register.City.Items = GetDictionaryValues("Cities");
			return au;
		}

		public bool SaveBook(RepBaseBook rb)
		{
			if (db.CheckRepetitionTime(rb))
			{
				Repetition r = new Repetition()
					{
						Comment = rb.Comment,
						MusicianId = UserData.CurrentUser.Id,
						RepBaseId = rb.RepBaseId,
						RoomId = rb.Room.Value,
						Status = (int) ViewModel.Status.ordered,
						TimeStart = rb.Time.Begin,
						TimeEnd = rb.Time.End,
						Date = rb.Date,
						Sum = db.GetRepetitionSum(rb)
					};
				db.AddRepetition(r);

				rb.Room.Items = db.GetDictionary("Rooms", rb.RepBaseId);
				sms.SendRepetitionIsBooked(rb, rb.Room.Display, db.GetRepBaseMaster(rb.RepBaseId).PhoneNumber);

				return true;
			}
			else return false;
		}

		public RepBase GetRepBase(int id)
		{
			var info = db.GetRepBase(id);
			if (info == null)
			{
				var ex = new RepaemNotFoundException("Репетиционная база не найдена!");
				throw ex;
			}
			return info;
		}

		public string GetRepBaseName(int id)
		{
			return db.GetBaseName(id);
		}

		public void CancelRepetition(int id)
		{
			var info = db.GetRepetitionInfo(id);
			sms.SendRepetitionIsCancelled(info.PhoneNumber, info.RoomName, info.RepBaseName, info.TimeStart, info.TimeEnd);
			email.SendRepetitionIsCancelled(info.Email, info.Name, info.Name, info.TimeStart, info.TimeEnd);

			db.SetRepetitionStatus(id, Status.cancelled);
		}

		public Comments GetRepBaseComments(int id)
		{
			var c = new Comments {RepBaseId = id, RepBaseName = db.GetBaseName(id), List = db.GetRepBaseComments(id)};
			return c;
		}

		public bool CheckCanRate(int id)
		{
			return UserData.CurrentUser != null ? 
				db.CheckCanCommentRepBase(id, UserData.CurrentUser.Id, HttpContext.Current.Request.UserHostAddress) : 
				db.CheckCanCommentRepBase(id, HttpContext.Current.Request.UserHostAddress);
		}
	}
}