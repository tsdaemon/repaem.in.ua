﻿using System;
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
            model.NewBases = db.GetNewBases().ToList();
            model.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;
            return View(model);
        }

        public ActionResult Feedback()
        {
            Feedback f = new Feedback();
            f.Previous.Add(new Feedback() { Name = "Вася", Email = "укукукук@erre.e", Text = "kjlllllllllllllllllllllllllllllllllllll" });
            //TODO: TO KCH Если пользователь авторизован, подставить данные имя/почта из профиля
            return View(f);
        }

        [HttpPost]
        public ActionResult Feedback(Feedback f)
        {
            //TODO: проверить капчу, проверить собщение (обычно через такие места чаще всего ломают, поэтому сканировать текст отзыва)
            //TODO: TO KCH сохранить сообщение
            ViewBag.Message = "Спасибо за отзыв!";
            return RedirectToAction("Index");
        }
    }
}
