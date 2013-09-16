using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class RepetitionController : RepaemAdminControllerBase
	{
		public RepetitionController(IManagerLogicProvider logic) : base(logic) { }

		//
		// GET: /Admin/Repetition/

		public ActionResult Index()
		{
			RepetitionIndex model = Logic.GetRepetitionIndex();
			return View(model);
		}
	}
}