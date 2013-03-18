using System;
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

            ModelBinders.Binders.Add(new KeyValuePair<Type,IModelBinder>(typeof(RepBaseFilter), new RepBaseFilterBinder()));
        }

        class RepBaseFilterBinder:DefaultModelBinder
        {
            public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                RepBaseFilter rep = (RepBaseFilter)base.BindModel(controllerContext, bindingContext);

                var t = new Range();
                t.Begin = int.Parse(controllerContext.HttpContext.Request["Time.Begin"]);
                t.End = int.Parse(controllerContext.HttpContext.Request["Time.End"]);
                rep.Time = t;

                var p = new Range();
                p.Begin = int.Parse(controllerContext.HttpContext.Request["Price.Begin"]);
                p.End = int.Parse(controllerContext.HttpContext.Request["Price.End"]);
                rep.Price = p;

                return rep;
            }
        }
    }
}
