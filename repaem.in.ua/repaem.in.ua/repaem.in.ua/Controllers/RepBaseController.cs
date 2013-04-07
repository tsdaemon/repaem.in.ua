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
            RepBaseList view = new RepBaseList(true);
            view.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(view);
        }

        [HttpPost]
        public ActionResult Index(RepBaseFilter filter)
        {
            //TODO: TO(KCH) Найти подходящие к фильтру базы. Я бы вынес это в отдельную функцию ;)
            RepBaseList view = new RepBaseList(true);
            view.Filter = filter;
            view.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(view);
        }

        [HttpPost]
        public ActionResult Search(RepBaseFilter filter)
        {
            var r = new RepBaseList(true);
            //TODO: TO(KCH) Найти подходящие к фильтру базы. Я бы вынес это в отдельную функцию ;)
            r.Filter = filter;
            r.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(r);
        }

        public ActionResult Repbase(int id)
        {
            //TODO: TO(KCH) найти ьазу по ид, заполнить вьювмодел и пихнуть во вьюху вместо демо записи
            return View(new RepBase(true));
        }

        //Замовлення бази з ід
        public ActionResult Book(int id)
        {
            return View();
        }
    }
}
