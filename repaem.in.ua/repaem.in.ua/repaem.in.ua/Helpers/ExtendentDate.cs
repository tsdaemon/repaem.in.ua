using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Helpers
{
    public static partial class Extensions
    {
        public static string ExtendentDate(this HtmlHelper html, DateTime d1)
        {
            try
            {
                DateTime d2 = DateTime.Today;
                if (d2 == d1.Date)
                    return "сегодня";
                else if (d2.AddDays(1.0) == d1.Date)
                    return "завтра";
                else
                    return d1.ToString("dddd, dd MMMM yyyy");
            }
            catch
            {
                return "";
            }
        }
    }
}