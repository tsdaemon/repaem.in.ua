using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Controllers
{
    public class LogicControllerBase : Controller
    {
        protected IRepaemLogicProvider Logic { get; set; }

        public LogicControllerBase(IRepaemLogicProvider _lg)
        {
            Logic = _lg;
        }
    }
}