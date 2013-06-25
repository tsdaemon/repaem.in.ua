using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using System.Web.Security;

namespace aspdev.repaem.Controllers
{
    public class RepBaseController : Controller
    {
        //
        // GET: /RepBase/

        public ActionResult Index()
        {
            //TODO: TO(KCH) В этом контроллере мы просто запихиваем в RepBaseList все базы, какие есть.
            RepBaseList view = new RepBaseList(true)
            {
                Filter = {DisplayTpe = RepBaseFilter.DisplayType.inline}
            };
            return View(view);
        }

        [HttpPost]
        public ActionResult Index(RepBaseFilter filter)
        {
            //TODO: TO(KCH) В этом контроллере мы просто запихиваем в RepBaseList все базы, которые подходят к фильтру.
            RepBaseList view = new RepBaseList(true) //Вместо этой строки 
            {
                Filter = filter
            };
            view.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(view);
        }

        [HttpPost]
        public ActionResult Search(RepBaseFilter filter)
        {
            var r = new RepBaseList(true);
            //TODO: TO(KCH) В этом контроллере мы просто запихиваем в RepBaseList все базы, которые подходят к фильтру.
            //Вбиваем предпологаемую дату в сессию, потом когда будем заказывать достанем его
            Session["book_date"] = filter.Date;
            Session["book_time"] = filter.Time;
            r.Filter = filter;
            r.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(r);
        }

        public ActionResult Repbase(int id)
        {
            //TODO: TO(KCH) найти базу по ид, заполнить вьювмодел и пихнуть во вьюху вместо демо записи
            return View(new RepBase(true));
        }

        //Замовлення бази з ід
        public ActionResult Book(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var book = new RepBaseBook(id);

                if(Session["base_date"]!=null)
                    book.Date = (DateTime)Session["base_date"];
                else
                    book.Date = DateTime.Today;

                book.Time = Session["base_time"] as TimeRange;
                if (book.Time == null)
                    book.Time = new TimeRange(12, 18);

                return View(book);
            }
            else
            {
                Session["base_id"] = id;
                return RedirectToAction("AuthOrRegister", "Account");
            }
        }

        //Відмінити репетицію 
        public JsonResult Cancel(int id)
        {
            //TODO: Відмінити репетицію, послати смс адміну
            //TODO: Створити джсон
            var p = new JsonResult();
            return new JsonResult();
        }

        public ActionResult Map()
        {
            //TODO: TO KCH заповнити список координат баз

            var Map = new GoogleMap(true);
            return View(Map);
        }
    }
}
