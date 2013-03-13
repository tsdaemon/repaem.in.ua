using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;

namespace aspdev.repaem.Controllers
{
    public class RepBaseController : Controller
    {
        //
        // GET: /RepBase/

        public ActionResult Index()
        {
            return View(new RepBaseIndexView());
        }

        [HttpPost]
        public ActionResult Index(RepBaseFilter filter)
        {
            return View(new RepBaseIndexView());
        }

    }
}
