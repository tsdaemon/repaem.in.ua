using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class HomeController : RepaemAdminControllerBase
	{
		//
		// GET: /Admin/Home/
		public HomeController(IManagerLogicProvider logic) : base(logic)
		{
		}

		[RepaemTitle(Title = "Администрирование - Главная")]
		public ActionResult Index()
		{
			return View(Logic.GetHomeIndex());
		}
	}
}