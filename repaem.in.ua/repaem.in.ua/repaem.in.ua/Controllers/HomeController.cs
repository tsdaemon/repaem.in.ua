using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Services;

namespace aspdev.repaem.Controllers
{
	public class HomeController : RepaemControllerBase
	{
		private ISession _session;

		public HomeController(IRepaemLogicProvider r, ISession s) : base(r)
		{
			_session = s;
		}

		public ActionResult Index()
		{
			var model = Logic.GetHomeIndexModel();
			return View(model);
		}

		//TODO: Delete on production!
		public string Demo()
		{
			return Logic.TryDemoData() ? "Sucess!" : "Fail!";
		}
	}
}