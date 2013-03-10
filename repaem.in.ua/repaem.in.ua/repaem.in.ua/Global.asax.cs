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

            ModelBinders.Binders.Add(typeof(Range), new RangeModelBinder());
        }
    }

    class RangeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Range))
                return null;

            string name = bindingContext.ModelName;
            object b = controllerContext.HttpContext.Request[String.Format("range-slider-{0}-val1", name)];
            object e = controllerContext.HttpContext.Request[String.Format("range-slider-{0}-val2", name)];

            Range r = new Range(int.Parse(b.ToString()), int.Parse(e.ToString()));

            return r;
        }
    }
}
