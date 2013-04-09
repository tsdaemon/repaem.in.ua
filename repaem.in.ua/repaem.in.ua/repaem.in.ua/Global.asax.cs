﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof(Register), new RegisterBinder()));
        }
    }

    public class RegisterBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object o = base.BindModel(controllerContext, bindingContext);
            Register re = o as Register;
            if (re.Capcha == null)
            {
                re.Name = controllerContext.HttpContext.Request["Register.Name"];
                re.Email = controllerContext.HttpContext.Request["Register.Email"];
                re.Password = controllerContext.HttpContext.Request["Register.Password"];
                re.Password2 = controllerContext.HttpContext.Request["Register.Password2"];
                re.Phone = controllerContext.HttpContext.Request["Register.Phone"];
                re.City.Value = int.Parse(controllerContext.HttpContext.Request["Register.City.Value"]);
                re.Capcha.Value = controllerContext.HttpContext.Request["Register.Capcha.Value"];
            }
            return re;
        }
    }
}

