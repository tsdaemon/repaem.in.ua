using System.Net;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class RepetitionController : RepaemAdminControllerBase
	{
		public RepetitionController(IManagerLogicProvider logic) : base(logic) { }

		//
		// GET: /Admin/Repetition/
		[RepaemTitle(Title = "Репетиции"), HttpGet]
		public ActionResult Index()
		{
			RepetitionIndex model = Logic.GetRepetitionIndex();
			return View(model);
		}

		[HttpDelete]
		public HttpStatusCodeResult Approve(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			Logic.ApproveRepetition(id);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[HttpDelete]
		public HttpStatusCodeResult Reject(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			Logic.RejectRepetition(id, false);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[HttpDelete]
		public HttpStatusCodeResult RejectOne(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			Logic.RejectRepetition(id, true);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[RepaemTitle(Title = "Редактировать репетицию"), HttpGet]
		public ViewResult Edit(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			return View(Logic.GetRepetitionEdit(id));
		}
	}
}