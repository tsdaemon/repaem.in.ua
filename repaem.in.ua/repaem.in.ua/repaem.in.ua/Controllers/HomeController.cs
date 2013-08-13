using System.Web.Mvc;
using aspdev.repaem.Services;

namespace aspdev.repaem.Controllers
{
	public class HomeController : LogicControllerBase
	{
		public HomeController(IRepaemLogicProvider r) : base(r)
		{
		}

		public ActionResult Index()
		{
			var model = Logic.GetHomeIndexModel();
			//TempData["Message"] = new Message() { Text = "repaem.in.ua приветствует Вас!", Caption = "Здраствуйте!", Color = new RepaemColor("green") };
			return View(model);
		}

		public JsonResult GetDistincts(int id)
		{
			var val = Logic.GetDictionaryValues("Distincts", id);
			return Json(val, JsonRequestBehavior.AllowGet);
		}

		//Delete on production!
		public string Demo()
		{
			return Logic.TryDemoData() ? "Sucess!" : "Fail!";
		}
	}
}