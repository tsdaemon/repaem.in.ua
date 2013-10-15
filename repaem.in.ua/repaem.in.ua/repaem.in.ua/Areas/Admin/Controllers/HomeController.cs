using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class HomeController : RepaemAdminControllerBase
	{
		//
		// GET: /Admin/Home/
		private RepetitionRepo _repo;

		public HomeController(RepaemManagerLogicProvider logic, RepetitionRepo repo) : base(logic)
		{
			_repo = repo;
		}

		[RepaemTitle(Title = "Администрирование - Главная")]
		public ActionResult Index()
		{
			return View(Logic.GetHomeIndex());
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