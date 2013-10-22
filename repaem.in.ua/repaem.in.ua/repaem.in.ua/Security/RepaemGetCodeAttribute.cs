using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using aspdev.repaem.Security;

namespace aspdev.repaem.Infrastructure
{
	public class RepaemGetCodeAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var us = DependencyResolver.Current.GetService<RepaemUserService>();

			if (us.CurrentUser == null || !us.CurrentUser.PhoneChecked)
				filterContext.Result = new RedirectToRouteResult("Default",
					new RouteValueDictionary() { { "controller", "Account" }, { "action", "GetCode" } }
					);
			else
				base.OnActionExecuting(filterContext);
		}
	}
}