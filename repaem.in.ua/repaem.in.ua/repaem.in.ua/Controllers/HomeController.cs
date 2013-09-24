using System.Web;
using System.Web.Mvc;
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
	}
}