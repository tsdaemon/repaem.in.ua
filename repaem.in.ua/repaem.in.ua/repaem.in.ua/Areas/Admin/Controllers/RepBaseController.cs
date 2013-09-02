using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class RepBaseController : RepaemAdminControllerBase
	{
		public RepBaseController(IManagerLogicProvider logic) : base(logic) { }

		[RepaemTitle(Title = "Комментарии")]
		public ActionResult Comments(int id)
		{
			var comments = Logic.GetRepBaseComments(id);
			ViewBag.Title = comments.RepBaseName;
			return View(comments);
		}
	}
}
