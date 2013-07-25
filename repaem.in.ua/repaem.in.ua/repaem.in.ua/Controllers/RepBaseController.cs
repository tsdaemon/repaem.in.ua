﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using System.Web.Security;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Infrastructure.Exceptions;

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
            RepBaseList view = Logic.GetAllRepBasesList();

            return View(view);
        }

        [HttpPost]
        public ActionResult Search(RepBaseFilter filter)
        {
            Session.Filter = filter;
            var r = Logic.GetRepBasesByFilter(filter);
            return View(r);
        }

        public ActionResult Repbase(int id)
        {
            return View(Logic.GetRepBase(id));
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

        [HttpPost, Authorize]
        public ActionResult Book(RepBaseBook rb)
        {
            if (ModelState.IsValid)
            {
                if (!Logic.SaveBook(rb)) {
                    TempData["Message"] = new Message()
                    {
                        Text = "Комната уже заказана! Попробуйте другую...",
                        Color = new RepaemColor("orange")
                    };
                    if (Session.Filter != null)
                        return RedirectToAction("Search");
                    else
                        return RedirectToAction("Index");
                }
                else 
                {
                    TempData["Message"] = new Message()
                    {
                        Text = "Спасибо за заказ!",
                        Color = new RepaemColor("green")
                    };
                    return RedirectToAction("Repetitions", "Account");
                }
            }
            else return View(rb);
        }

        //Відмінити репетицію 
        [Authorize]
        public JsonResult Cancel(int id)
        {
            try 
            {
                Logic.CancelRepetition(id);
                var p = new JsonResult();
                p.Data = new { Result = "success" };
                return new JsonResult();
            }
            catch (RepaemException re) 
            {
                var p = new JsonResult();
                p.Data = new { Result = "fail", Message = re.Message };
                return new JsonResult();
            }
        }

        public ActionResult Map()
        {
            var Map = new GoogleMap() { Coordinates = Logic.GetAllBasesCoordinates() };
            return View(Map);
        }

        //Залишити відгук
        [HttpGet]
        public ActionResult Rate(int id, double rating)
        {
            var cm = new aspdev.repaem.ViewModel.Comment();
            cm.RepBaseId = id;
            cm.RepBaseName = Logic.GetRepBaseName(id);
            cm.Rating = rating;
            if (User.Identity.IsAuthenticated)
            {
                cm.Email = Logic.UserData.CurrentUser.Email;
                cm.Name = Logic.UserData.CurrentUser.Name;
            }
            
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
                Logic.SaveComment(c);
                TempData["Message"] = new Message()
                {
                    Text = "Ваш комментарий добавлен!",
                    Color = new RepaemColor("green")
                };
                return RedirectToAction("Repbase", new { id = c.RepBaseId });
            }
            else return View(c);
        }
    }
}
