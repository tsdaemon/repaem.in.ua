using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            HomeIndexModel model = new HomeIndexModel(true);
            return View(model);
        }

        public ActionResult Test()
        {
            //TODO: удалить после заврешения
            ViewBag.Message = "Спасибо за отзыв!";
            return View();
        }

        public ActionResult Feedback()
        {
            Feedback f = new Feedback();
            f.Previous.Add(new Feedback() { Name = "Вася", Email = "укукукук@erre.e", Text = "kjlllllllllllllllllllllllllllllllllllll" });
            //TODO: Если пользователь авторизован, подставить данные имя/почта из профиля
            return View(f);
        }

        [HttpPost]
        public ActionResult Feedback(Feedback f)
        {
            //TODO: проверить капчу, проверить собщение (обычно через такие места чаще всего ломают, поэтому сканировать текст отзыва)
            ViewBag.Message = "Спасибо за отзыв!";
            return RedirectToAction("Index");
        }
    }
}
