using aspdev.repaem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Controllers
{
    public class CommentsController : Controller
    {
        [HttpPost]
        //Асинхронный результат голосования. Смотри документацию к класу Vote
        public JsonResult Vote(Vote v)
        {
            //TODO: BY KCH: Вернуть JSON id коммента
            return new JsonResult();
        }

        public ViewResult CommentDialog(int id)
        {
            //TODO: BY KCH: Отримати коммент по номеру та передати у вьюху або створити новий
            //TODO: BY AST: Зробити вьюху для комментів
            return View();
        }
    }
}
