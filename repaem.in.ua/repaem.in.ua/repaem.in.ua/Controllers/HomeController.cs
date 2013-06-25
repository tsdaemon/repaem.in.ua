using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        Database db = new Database();

        public ActionResult Index()
        {
            HomeIndexModel model = new HomeIndexModel();
            model.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;

            return View(model);
        }
    }
}
