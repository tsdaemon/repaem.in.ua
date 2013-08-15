using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using System.Web.Security;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Infrastructure.Exceptions;

namespace aspdev.repaem.Controllers
{
	public class RepBaseController : RepaemControllerBase
	{
		//
		// GET: /RepBase/
		private readonly ISession _session;

		public RepBaseController(IRepaemLogicProvider p, ISession s) : base(p)
		{
			_session = s;
		}

		public ActionResult Index()
		{
			RepBaseList view = Logic.GetAllRepBasesList();

			return View(view);
		}

		[HttpPost]
		public ActionResult Search(RepBaseFilter filter)
		{
			_session.Filter = filter;
			var r = Logic.GetRepBasesByFilter(filter);
			return View(r);
		}

		public ActionResult Display(int id)
		{
			return View(Logic.GetRepBase(id));
		}

		//Замовлення бази з ід
		//public ActionResult Book(int id)
		//{
		//	if (User.Identity.IsAuthenticated)
		//	{
		//		var book = Logic.GetRepBaseBook(id);
		//		return View(book);
		//	}
		//	else
		//	{
		//		session.BookBaseId = id;
		//		return RedirectToAction("AuthOrRegister", "Account");
		//	}
		//}

		//Замовлення бази з ід, датою, часом
		public ActionResult Book(int id, DateTime? datetime, int? roomid)
		{
			if (User.Identity.IsAuthenticated)
			{
				if (datetime.HasValue)
				{
					var t = datetime.Value;
					var hour = 0;
					if (t.Hour > 0)
						hour = t.Hour;
					else if (_session.BookTime != null)
						hour = _session.BookTime.Begin;

					return View(Logic.GetRepBaseBook(id, t.Date, hour, roomid.Value));
				}
				else 
					return View(Logic.GetRepBaseBook(id));
			}
			else
			{
				_session.BookBaseId = id;
				if (datetime.HasValue)
				{
					var t = datetime.Value;
					_session.BookDate = t.Date;
					_session.BookTime = new TimeRange {Begin = t.Hour, End = t.Hour + 2};
					_session.BookRoomId = roomid;
				}
				return RedirectToAction("AuthOrRegister", "Account");
			}
		}

		[HttpPost, Authorize]
		public ActionResult Book(RepBaseBook rb)
		{
			if (ModelState.IsValid)
			{
				if (!Logic.SaveBook(rb))
				{
					ModelState.AddModelError("Time", "Время уже занято! Попробуйте другое");
					Logic.UpdateRepBaseBook(rb);
					return View(rb);
				}
				else
				{
					TempData["Message"] = new Message()
						{
							Text = "Спасибо за заказ!",
							Color = new RepaemColor("green")
						};
					return RedirectToAction("Repetitions", "Account");
				}
			}
			else return View(rb);
		}

		//Відмінити репетицію 
		[Authorize]
		public bool Cancel(int id)
		{
			try
			{
				Logic.CancelRepetition(id);
				var p = new JsonResult();
				return true;
			}
			catch (RepaemException re)
			{
				return false;
			}
		}

		public ActionResult Map()
		{
			var map = new GoogleMap() {Coordinates = Logic.GetAllBasesCoordinates()};
			return View(map);
		}

		//Залишити відгук
		[HttpGet]
		public ActionResult Rate(int id, double rating)
		{
			var cm = new ViewModel.Comment
				{
					RepBaseId = id,
					RepBaseName = Logic.GetRepBaseName(id),
					Rating = rating
				};
			if (User.Identity.IsAuthenticated)
			{
				cm.Email = Logic.UserData.CurrentUser.Email;
				cm.Name = Logic.UserData.CurrentUser.Name;
			}

			return View(cm);
		}

		[HttpPost]
		public ActionResult Rate(ViewModel.Comment c)
		{
			if (c.Capcha.Value != _session.Capcha)
			{
				ModelState.AddModelError("Capcha", "Неправильная капча!");
			}

			if (ModelState.IsValid)
			{
				Logic.SaveComment(c);
				TempData["Message"] = new Message()
					{
						Text = "Ваш комментарий добавлен!",
						Color = new RepaemColor("green")
					};
				return RedirectToAction("Display", new {id = c.RepBaseId});
			}
			else return View(c);
		}

		//Подивитися відгуки
		public ActionResult Comments(int id)
		{
			return View(Logic.GetRepBaseComments(id));
		}
	}
}