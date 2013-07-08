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
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        Database db = new Database();
        ILogger log;

        public HomeController(ILogger _log)
        {
            log = _log;
        }

        public ActionResult Index()
        {
            HomeIndexModel model = new HomeIndexModel();
            model.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;

            return View(model);
        }

        //Delete on production!
        public string DemoData()
        {
            db.CreateDemoData();
            return "Sucess!";
        }

        public string DeleteDemoData()
        {
            db.DeleteDemoData();
            return "Sucess!";
        }
    }
}
