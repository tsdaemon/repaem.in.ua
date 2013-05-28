using aspdev.repaem.Areas.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Areas.Admin.Controllers
{
    public class RepBaseController : Controller
    {
        public ActionResult List()
        {
            //TODO: TO KCH Заполнить список реальными базами для этого пользователя
            List<RepBaseListItem> ls = new List<RepBaseListItem>();
            ls.Add(new RepBaseListItem() { Id = 1, Address = "Красноткацкая, 14", Name = "Старый рыбак", Rating = 2.6f });
            ls.Add(new RepBaseListItem() { Id = 2, Address = "Красноткацкая, 14", Name = "Старый рыбак", Rating = 2.6f });
            return View(ls);
        }

        public ActionResult Add()
        {
            return View(new RepBaseAddEdit());
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public void Delete(int id)
        {
            //TODO: TO KCH - видалити базу з данним ід
        }
    }
}
