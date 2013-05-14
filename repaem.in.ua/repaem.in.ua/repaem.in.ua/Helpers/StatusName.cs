using aspdev.repaem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Helpers
{
    public static partial class Extensions
    {
        public static string StatusName(this HtmlHelper html, Status st)
        {
            switch (st)
            {
                case Status.approoved: return "Подтверждена";
                case Status.cancelled: return "Отменена";
                case Status.constant: return "Постоянная";
                case Status.ordered: return "Заказана";
                default: return "";
            }
        }
    }
}