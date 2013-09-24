using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class RepBaseController : RepaemAdminControllerBase
	{
		public RepBaseController(RepaemManagerLogicProvider logic) : base(logic) { }

		[RepaemTitle(Title = "Комментарии")]
		[HttpGet]
		public ActionResult Comments(int id)
		{
			var comments = Logic.GetRepBaseComments(id);
			ViewBag.Title = comments.RepBaseName;
			return View(comments);
		}

		[RepaemTitle(Title = "Список репетиционных баз")]
		[HttpGet]
		public ActionResult Index()
		{
			var bases = Logic.GetRepBasesList();
			return View(bases);
		}

		[RepaemTitle(Title = "Редактировать базу")]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			RepBaseEdit edit = Logic.GetRepBaseEditModel(id);
			return View(edit);
		}

		[RepaemTitle(Title = "Редактировать базу")]
		[HttpPost]
		public ActionResult Edit(RepBaseEdit edit)
		{
			if (ModelState.IsValid)
			{
				Logic.SaveRepBase(edit);
			}
			Logic.PrepareRepBaseEdit(edit);
			return View(edit);
		}

		[RepaemTitle(Title = "Создать новую базу")]
		[HttpGet]
		public ActionResult Create()
		{
			RepBaseEdit edit = new RepBaseEdit();
			Logic.PrepareRepBaseEdit(edit);
			return View(edit);
		}

		[RepaemTitle(Title = "Создать новую базу")]
		[HttpPost]
		public ActionResult Create(RepBaseEdit edit)
		{
			if (ModelState.IsValid)
			{
				Logic.AddRepBase(edit);
				Logic.PrepareRepBaseEdit(edit);
				return RedirectToAction("Edit", new { id = edit.Id });
			}
			else
			{
				Logic.PrepareRepBaseEdit(edit);
				return View(edit);
			}
		}
	}
}
