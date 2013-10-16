using System;
using System.Web.Mvc;
using aspdev.repaem.Models;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Controllers
{
	public class RepBaseController : RepaemControllerBase
	{
		//
		// GET: /RepBase/
		private readonly ISession _session;

		public RepBaseController(RepaemLogicProvider p, ISession s) : base(p)
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

		//Замовлення бази з ід, датою, часом
		[HttpGet]
		public ActionResult Book(int id)
		{
			if (User.Identity.IsAuthenticated)
			{
				return View(Logic.GetRepBaseBook(id));
			}
			else
			{
				_session.BookBaseId = id;
				return RedirectToAction("AuthOrRegister", "Account");
			}
		}

		[HttpGet]
		public ActionResult BookRoom(int roomid, DateTime datetime)
		{
			int id = Logic.GetRepBaseByRoom(roomid);
			if (User.Identity.IsAuthenticated)
			{
				return View("Book", Logic.GetRepBaseBook(id, datetime, roomid));
			}
			else
			{
				_session.BookBaseId = id;
				return RedirectToAction("AuthOrRegister", "Account");
			}
		}

		[HttpPost, RepaemAuth]
		public ActionResult Book(RepBaseBook rb)
		{
			if (ModelState.IsValid)
			{
                //TODO: переделать под исключения
                try 
                {
				    Logic.SaveBook(rb);
                }
                catch(RepaemException e)
				{
					ModelState.AddModelError(e.ModelKey, e);
					Logic.UpdateRepBaseBook(rb);
					return View(rb);
				}
				TempData["Message"] = new Message()
					{
						Text = "Спасибо за заказ!",
						Color = new RepaemColor("green")
					};
				return RedirectToAction("Repetitions", "Account");
			}
			else return View(rb);
		}

		//Відмінити репетицію 
		[RepaemAuth]
		public bool Cancel(int id)
		{
			try
			{
				Logic.CancelRepetition(id);
				return true;
			}
			catch (RepaemException)
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
			if (Logic.CheckCanRate(id))
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
			else
			{
				TempData["Message"] = new Message() {Text = "Вы уже голосовали за эту базу!", Color = new RepaemColor("orange")};
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public ActionResult Rate(Comment c)
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