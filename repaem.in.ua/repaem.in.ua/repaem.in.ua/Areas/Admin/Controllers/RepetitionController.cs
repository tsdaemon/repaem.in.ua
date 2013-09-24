using System;
using System.Net;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class RepetitionController : RepaemAdminControllerBase
	{
		public RepetitionController(RepaemManagerLogicProvider logic) : base(logic) { }

		//
		// GET: /Admin/Repetition/
		[RepaemTitle(Title = "Репетиции"), HttpGet]
		public ActionResult Index()
		{
			RepetitionIndex model = Logic.GetRepetitionIndex();
			return View(model);
		}

		[HttpPost]
		public HttpStatusCodeResult Approve(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			Logic.ApproveRepetition(id);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[HttpPost]
		public HttpStatusCodeResult Reject(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			Logic.RejectRepetition(id);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[HttpPost]
		public HttpStatusCodeResult RejectOne(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			Logic.RejectRepetition(id);

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[RepaemTitle(Title = "Редактировать репетицию"), HttpGet]
		public ViewResult Edit(int id)
		{
			Logic.CheckPermissions(id, "Repetition");
			return View(Logic.GetRepetitionEdit(id));
		}

		[RepaemTitle(Title = "Редактировать репетицию"), HttpPost]
		public ViewResult Edit(RepetitionEdit edit)
		{
			Logic.CheckPermissions(edit.Id, "Repetition");
			Logic.PrepareRepetitionEdit(edit);
			if (Logic.CheckRepetitionTime(edit))
			{
				ModelState.AddModelError("Time", "Это время уже занято!");
			}
			if (ModelState.IsValid)
			{
				Logic.SaveRepetitionEdit(edit);
			}
			return View(edit);
		}

		[RepaemTitle(Title = "Создать репетицию"), HttpGet]
		public ViewResult Create()
		{
			var edit = new RepetitionEdit {Date = DateTime.Today, Time = new TimeRange(12, 14)};

			Logic.PrepareRepetitionEdit(edit);
			return View(edit);
		}

		[RepaemTitle(Title = "Создать репетицию"), HttpPost]
		public ViewResult Create(RepetitionEdit edit)
		{
			Logic.PrepareRepetitionEdit(edit);
			if (!Logic.CheckRepetitionTime(edit))
			{
				ModelState.AddModelError("Time", "Это время уже занято!");
			}
			if (ModelState.IsValid)
			{
				Logic.CreateRepetition(edit);
				return View("Edit", edit);
			}
			else 
			{
				return View(edit);			
			}
		}

		[HttpGet]
		public JsonResult GetRooms(int id)
		{
			var rooms = Logic.GetRooms(id);
			return Json(rooms, JsonRequestBehavior.AllowGet);
		}

		[RepaemTitle(Title = "История репетиций"), HttpGet]
		public ViewResult History()
		{
			return View(Logic.GetHistory());
		}
	}
}