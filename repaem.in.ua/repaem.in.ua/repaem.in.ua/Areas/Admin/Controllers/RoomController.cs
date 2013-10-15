using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure;
using aspdev.repaem.Models.Data;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.Controllers
{
  public class RoomController : RepaemAdminControllerBase
	{
		public RoomController(RepaemManagerLogicProvider logic) : base(logic) { }

		//
		// GET: /Admin/Room/
		[RepaemTitle(Title = "Комнаты")]
    public ActionResult Index()
    {
			var model = Logic.GetRooms();
      return View(model);
    }

		[RepaemTitle(Title = "Редактировать комнату")]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			RoomEdit model = Logic.GetRoomEdit(id);
			return View(model);
		}

		[RepaemTitle(Title = "Редактировать комнату")]
		[HttpPost]
		public ActionResult Edit(RoomEdit edit)
		{
			Logic.CheckPermissions(edit.Id, "Room");
			if (ModelState.IsValid)
			{
				Logic.SaveRoom(edit);
			}
			Logic.PrepareRoomEdit(edit);
			return View(edit);
		}

		[RepaemTitle(Title = "Добавить комнату")]
		[HttpGet]
		public ActionResult Create(int? id)
		{
			if(id.HasValue)
				Logic.CheckPermissions(id.Value, "RepBase");

			RoomEdit edit = Logic.CreateRoomEdit(id);
			return View(edit);
		}

		[RepaemTitle(Title = "Добавить комнату")]
		[HttpPost]
		public ActionResult Create(RoomEdit edit)
		{
			Logic.CheckPermissions(edit.RepBaseId, "RepBase");
			Logic.PrepareRoomEdit(edit);
			if (ModelState.IsValid)
			{
				Logic.AddRoom(edit);
				return RedirectToAction("Edit", new {id = edit.Id});
			}
			else
			{
				return View(edit);
			}
		}

		[HttpDelete]
		public bool DeletePrice(int id)
		{
			try
			{
				Logic.CheckPermissions(id, "Price");

				Logic.DeletePrice(id);
			}
			catch
			{
				return false;
			}

			return true;
		}

		[HttpPut]
		public bool CreatePrice(int id)
		{
			//Так не катит, поэтому все таки релоад
			var pr = Logic.CreatePrice(id);
			return true;
		}
  }
}
