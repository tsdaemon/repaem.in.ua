using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Controllers;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class HomeController : RepaemAdminControllerBase
	{
		//
		// GET: /Admin/Home/

		public HomeController(IManagerLogicProvider logic) : base(logic)
		{
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}