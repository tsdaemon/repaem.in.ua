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
using aspdev.repaem.Models;

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

        void SaveBook(RepBaseBook rb);

        RepBase GetRepBase(int id);

        string GetRepBaseName(int id);

        void CancelRepetition(int id, bool? one, UserRole by);

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
		private readonly IDatabase _db;
		private readonly ISession _ss;
        private readonly IMessagesProvider _msg;

		public RepaemLogicProvider(IDatabase db, ISession ss, IUserService us, IMessagesProvider msg)
		{
			_db = db;
			_ss = ss;
			UserData = us;
            _msg = msg;
		}

		public IUserService UserData { get; private set; }

		public List<SelectListItem> GetDictionaryValues(string name)
		{
			var ls = _db.GetDictionary(name);
			ls.Insert(0, new SelectListItem() {Text = "", Value = "0"});
			return ls;
		}

		public List<SelectListItem> GetDictionaryValues(string name, int fKey)
		{
			var ls = _db.GetDictionary(name, fKey);
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
			var p = _db.GetProfile(UserData.CurrentUser.Id);
			p.City.Items = GetDictionaryValues("Cities");
			return p;
		}

		public RepBaseList GetAllRepBasesList()
		{
			RepBaseList l = new RepBaseList();
			l.Filter = GetFilter();
			l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
			l.Map = new GoogleMap();
			l.Map.Coordinates = _db.GetAllBasesCoordinates();
			l.RepBases = _db.GetAllBases();
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
				RepBaseName = _db.GetBaseName(id),
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
			return _db.GetAllBasesCoordinates();
		}

		public HomeIndexModel GetHomeIndexModel()
		{
			HomeIndexModel m = new HomeIndexModel();
			m.NewBases = _db.GetNewBases().ToList();
			m.Map = new GoogleMap() {Coordinates = _db.GetAllBasesCoordinates()};
			m.Filter = GetFilter();
			m.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;
			return m;
		}

		public Profile GetUserProfile()
		{
			if (UserData.CurrentUser != null)
			{
				var pf = _db.GetProfile(UserData.CurrentUser.Id);
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
			var reps = from r in _db.GetRepetitions(UserData.CurrentUser.Id)
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

			_db.SaveComment(c1);
		}

		public AuthOrRegister GetAuthOrRegister()
		{
			var au = new AuthOrRegister();
			au.Register.City.Items = GetDictionaryValues("Cities");
			return au;
		}

    public void SaveBook(RepBaseBook rb)
		{
      //don't play with time...
      DateTime begin = rb.Date.AddHours(rb.Time.Begin);
      if (DateTime.Now <= begin)
				throw new RepaemItIsPastException("Не пытайтесь обмануть время");

      if (!_db.CheckRepetitionTime(rb))
				throw new RepaemTimeIsBusyException("Это время уже занято! Попробуйте другое!");

			if(!UserData.CurrentUser.PhoneChecked)
				throw new RepaemPhoneNotCheckedException("Музыкант с непроверенным номером не может заказывать репетици");
			
			Repetition r = new Repetition()
			{
				Comment = rb.Comment,
				MusicianId = UserData.CurrentUser.Id,
				RepBaseId = rb.RepBaseId,
				RoomId = rb.Room.Value,
				Status = (int) Status.ordered,
				TimeStart = rb.Time.Begin,
				TimeEnd = rb.Time.End,
				Date = rb.Date,
				Sum = _db.GetRepetitionSum(rb)
			};
			_db.AddRepetition(r);

			//Сообщение о заказе
			rb.Room.Items = _db.GetDictionary("Rooms", rb.RepBaseId);
	    var master = _db.GetRepBaseMaster(rb.RepBaseId);
	    var musician = UserData.CurrentUser;

	    string msg =
		    String.Format("Заказана репетиция: репетиционная база {0}, комната {1}, {6} {2}.00-{3}.00, музыкант {4} {5}",
				rb.RepBaseName, rb.Room.Display, rb.Time.Begin, rb.Time.End, musician.Name, musician.PhoneNumber, rb.Date);
			_msg.SendMessage(msg, new string[] { master.PhoneNumber }, new string[]{ master.Email, musician.Email });
		}

		public RepBase GetRepBase(int id)
		{
			var info = _db.GetRepBase(id);
			return info;
		}

		public string GetRepBaseName(int id)
		{
			return _db.GetBaseName(id);
		}

		public void CancelRepetition(int id, bool? one, UserRole by)
		{
			var rep = _db.GetRepetitionInfo(id);
      var manager = _db.GetRepBaseMaster(rep.RepBaseId);

      var musician = _db.GetOne<User>(rep.MusicianId);
      var room = _db.GetOne<Room>(rep.RoomId);
      var repBase = _db.GetOne<RepBase>(rep.RepBaseId);

      string msg;

			switch (rep.Status)
			{
					//постійна репетиція
				case 2:
					//відміняємо на один раз
					if (one.HasValue && one.Value)
					{
						msg = String.Format("Постоянная репетиция на базе {0}, комната {1}, время {2}.00-{3}.00 {4} отменена на один раз",
						                    repBase.Name, room.Name, rep.TimeStart, rep.TimeEnd, rep.Date.DayOfWeek.ToString("dddd"));
						_db.CancelFixedRepOneTime(id);
					}
						//відміняємо назавжди
					else
					{
						msg = String.Format("Постоянная репетиция на базе {0}, комната {1}, время {2}.00-{3}.00 {4} отменена навсегда",
						                    repBase.Name, room.Name, rep.TimeStart, rep.TimeEnd, rep.Date.DayOfWeek.ToString("dddd"));
						_db.SetRepetitionStatus(id, Status.cancelled);
					}
					break;
					//звичайна репетиція
				case 1:
					msg = String.Format("Репетиция на базе {0}, комната {1}, время {2}.00-{3}.00 {4} отменена", 
															repBase.Name, room.Name, rep.TimeStart, rep.TimeEnd, rep.Date);
					_db.SetRepetitionStatus(id, Status.cancelled);
					break;
				default:
					throw new RepaemRepetitionWrongStatusException("Невозможно отменить репетицию c неверным статусом") { Status = (Status)rep.Status };
			}
      //определяем кому слать оповещения
      switch (by)
      {
          case UserRole.Musician: _msg.SendMessage(msg, new string[] { manager.PhoneNumber }, new string[] { manager.Email });
              break;
          case UserRole.Manager: _msg.SendMessage(msg, new string[] { musician.PhoneNumber }, new string[] { musician.Email });
              break;
      }
		}

		public Comments GetRepBaseComments(int id)
		{
			var c = new Comments {RepBaseId = id, RepBaseName = _db.GetBaseName(id), List = _db.GetRepBaseComments(id)};
			return c;
		}

		public bool CheckCanRate(int id)
		{
			return UserData.CurrentUser != null ? 
				_db.CheckCanCommentRepBase(id, UserData.CurrentUser.Id, HttpContext.Current.Request.UserHostAddress) : 
				_db.CheckCanCommentRepBase(id, HttpContext.Current.Request.UserHostAddress);
		}
	}
}