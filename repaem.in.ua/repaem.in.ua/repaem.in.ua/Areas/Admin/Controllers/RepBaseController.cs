using aspdev.repaem.Areas.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Areas.Admin.Controllers
{
    public class RepBaseController : Controller
    {
        //
        // GET: /Admin/Home/
        public ActionResult List()
        {
            //TODO: Придумать, как запихивать юзеринфо в каждую страницу
            UserInfo ui = new UserInfo() { Id = 1, Name = "Анатолий", NewBill = true, NewReps = 3 };
            ViewBag.UserInfo = ui;
            return View();
        }

    }
}
