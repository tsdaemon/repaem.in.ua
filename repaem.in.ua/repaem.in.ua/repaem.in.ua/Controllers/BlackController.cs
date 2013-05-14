using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Controllers
{
    public class BlackController : Controller
    {
        //
        // GET: /Black/

        public ActionResult Index()
        {
            //TODO: вернуть список всех музыкантов из черного списка
            return View();
        }

        public ActionResult One(int id)
        {
            //TODO: проверить есть ли этот ид в списке, вренуть список только с этим музыкантом
            return View();
        }

        public ActionResult Search(string pattern)
        {
            //TODO: Организовать поиск по черному листу
            return View();
        }
    }
}
