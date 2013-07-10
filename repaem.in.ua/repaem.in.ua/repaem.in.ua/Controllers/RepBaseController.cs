using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using System.Web.Security;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Controllers
{
    public class RepBaseController : LogicControllerBase
    {
        //
        // GET: /RepBase/
        ISession Session;
        public RepBaseController(IRepaemLogicProvider _p, ISession _s) : base(_p) { Session = _s; }

        public ActionResult Index()
        {
            RepBaseList view = new RepBaseList();
            view.LoadAllBases();

            return View(view);
        }

        [HttpPost]
        public ActionResult Search(RepBaseFilter filter)
        {
            var r = new RepBaseList();
            r.LoadBases(filter);

            //Вбиваем предпологаемую дату в сессию, потом когда будем заказывать достанем его
            Session.BookDate = filter.Date;
            Session.BookTime = filter.Time;
            r.Filter = filter;
            r.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            return View(r);
        }

        public ActionResult Repbase(int id)
        {
            return View(Logic.GetRepBaseBook(id));
        }

        //Замовлення бази з ід
        public ActionResult Book(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var book = Logic.GetRepBaseBook(id);
                return View(book);
            }
            else
            {
                Session.BookBaseId = id;
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
            var Map = new GoogleMap() { Coordinates = Logic.GetAllBasesCoordinates() };
            return View(Map);
        }

        //Залишити відгук
        public ActionResult Rate(int id, double rating)
        {
            var cm = new aspdev.repaem.ViewModel.Comment();
            cm.RepBaseId = id;
            cm.Rating = rating;
            
            return View(cm);
        }

        [HttpPost]
        public ActionResult Rate(aspdev.repaem.ViewModel.Comment c)
        {
            if (c.Capcha.Value != Session.Capcha) 
            {
                ModelState.AddModelError("Capcha", "Неправильная капча!");   
            }

            if (ModelState.IsValid)
            {
                c.SaveComment();
                ViewBag.Message = "Ваш комментарий добавлен!";
                //TODO: редирект на то, что было перед комментарием
                return RedirectToAction("Index", "Home");
            }
            else return View(c);
        }
    }
}
