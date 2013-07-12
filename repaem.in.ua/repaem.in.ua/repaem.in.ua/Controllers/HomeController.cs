using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Infrastructure.Exceptions;
using System.Web.Http.Controllers;

namespace aspdev.repaem.Controllers
{
    public class HomeController : LogicControllerBase
    {
        public HomeController(IRepaemLogicProvider r) : base(r) { }

        public ActionResult Index()
        {
            HomeIndexModel model = Logic.GetHomeIndexModel();
            return View(model);
        }

        public JsonResult GetDistincts(int id)
        {
            var val = Logic.GetDictionaryValues("Distincts", id);
            return Json(val, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Feedback()
        {
            return View();
        }

        //Delete on production!
        public string Demo()
        {
            if (Logic.TryDemoData())
                return "Sucess!";
            else
                return "Fail!";
        }
    }
}
