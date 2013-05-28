using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.ViewModel;

namespace aspdev.repaem.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/

        public ActionResult Info()
        {
            //TODO TO KCH Замінити тестову інформацію про користувача на реальну
            UserInfo ui = new UserInfo() { Id = 1, Name = "Анатолий", UnpaidBill = true, NewReps = 3 };
            return PartialView(ui);
        }

    }
}
