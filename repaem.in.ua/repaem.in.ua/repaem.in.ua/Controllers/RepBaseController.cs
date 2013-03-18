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
            RepBaseView view = new RepBaseView(true);
            view.Filter.DisplayTpe = RepBaseFilter.DisplayType.Inline;
            return View(new RepBaseView());
        }

        [HttpPost]
        public ActionResult Search(RepBaseFilter filter)
        {
            var r = new RepBaseView(true);
            r.Filter = filter;
            r.Filter.DisplayTpe = RepBaseFilter.DisplayType.Inline;
            return View(r);
        }
    }
}
