using System;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Infrastructure;
using aspdev.repaem.Services;

namespace aspdev.repaem.Controllers
{
	public class HomeController : RepaemControllerBase
	{
		public HomeController(RepaemLogicProvider r) : base(r)
		{
		}

		public ActionResult Index()
		{
			var model = Logic.GetHomeIndexModel();
			return View(model);
		}
		
		[RepaemTitle(Title = "Ошибка!")]
		public ActionResult Error()
		{
			return View();
		}

		[RepaemTitle(Title = "Страница не найдена")]
		public ActionResult Error404()
		{
			return View();
		}
	}
}