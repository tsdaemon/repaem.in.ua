using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace aspdev.repaem.Infrastructure
{
	public class RepaemAuthAttribute : AuthorizeAttribute
	{
		public string Roles { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var isAuthorized = base.AuthorizeCore(httpContext);
			if (!isAuthorized)
				return false;

			if (String.IsNullOrEmpty(Roles))
				return true;

			var rls = Roles.Split(new[] {", ", ","}, StringSplitOptions.RemoveEmptyEntries);

			return (from r in rls
			        where httpContext.User.IsInRole(r)
			        select true).FirstOrDefault();
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				filterContext.HttpContext.Session["backurl"] = filterContext.HttpContext.Request.Url.PathAndQuery;
				filterContext.Result = new RedirectToRouteResult("Default", new
					RouteValueDictionary(new { controller = "Account", action = "Auth" }));
			}
			else
			{
				base.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}