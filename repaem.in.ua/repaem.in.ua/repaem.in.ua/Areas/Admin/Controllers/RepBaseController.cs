using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;
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

		[RepaemTitle(Title = "Список репетиционных баз")]
		public ActionResult Index()
		{
			var bases = Logic.GetRepBasesList();
			return View(bases);
		}

		[RepaemTitle(Title = "Редактировать базу")]
		public ActionResult Edit(int id)
		{
			RepBaseEdit edit = Logic.GetRepBaseEditModel(id);
			return View(edit);
		}
	}
}
