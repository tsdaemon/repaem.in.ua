using System.Web.Mvc;
using aspdev.repaem.Services;

namespace aspdev.repaem.Controllers
{
	public class HomeController : RepaemControllerBase
	{
		public HomeController(IRepaemLogicProvider r) : base(r)
		{
		}

		public ActionResult Index()
		{
			var model = Logic.GetHomeIndexModel();
			return View(model);
		}

		public JsonResult GetDistincts(int id)
		{
			var val = Logic.GetDictionaryValues("Distincts", id);
			return Json(val, JsonRequestBehavior.AllowGet);
		}

		//TODO: Delete on production!
		public string Demo()
		{
			return Logic.TryDemoData() ? "Sucess!" : "Fail!";
		}
	}
}