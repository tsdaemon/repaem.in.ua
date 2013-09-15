using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure;

namespace aspdev.repaem.Areas.Admin.Controllers
{
  public class RoomController : RepaemAdminControllerBase
	{
		public RoomController(IManagerLogicProvider logic) : base(logic) { }

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
			
		}

		[RepaemTitle(Title = "Редактировать комнату")]
		[HttpPost]
		public ActionResult Edit(RoomEdit edit)
		{
			
		}

		[RepaemTitle(Title = "Добавить комнату")]
		[HttpGet]
		public ActionResult Create(int id)
		{
			
		}

		[RepaemTitle(Title = "Комнаты")]
		public ActionResult Create(RepBaseEdit edit)
		{

		}

  }
}
