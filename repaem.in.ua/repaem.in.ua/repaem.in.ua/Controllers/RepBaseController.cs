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
            //TODO: TO(KCH) Найти подходящие к фильтру базы. Я бы вынес это в отдельную функцию ;)
            RepBaseView view = new RepBaseView(true);
            view.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(view);
        }

        [HttpPost]
        public ActionResult Index(RepBaseFilter filter)
        {
            //TODO: TO(KCH) Найти подходящие к фильтру базы. Я бы вынес это в отдельную функцию ;)
            RepBaseView view = new RepBaseView(true);
            view.Filter = filter;
            view.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(view);
        }

        [HttpPost]
        public ActionResult Search(RepBaseFilter filter)
        {
            var r = new RepBaseView(true);
            //TODO: TO(KCH) Найти подходящие к фильтру базы. Я бы вынес это в отдельную функцию ;)
            r.Filter = filter;
            r.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(r);
        }
    }
}
