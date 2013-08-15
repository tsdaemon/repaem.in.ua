using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class HomeController : RepaemAdminControllerBase
	{
		//
		// GET: /Admin/Home/

		public HomeController(IAdminLogicProvider logic) : base(logic)
		{
		}

		public ActionResult Index()
		{
			return View(_logic.GetHomePage());
		}
	}
}