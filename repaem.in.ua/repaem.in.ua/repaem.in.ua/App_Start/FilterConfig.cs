﻿using aspdev.repaem.Infrastructure.Exceptions;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new ExceptionHandlingAttribute());
        }
    }
}