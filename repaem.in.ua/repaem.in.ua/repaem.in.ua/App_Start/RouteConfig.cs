using System.Web.Mvc;
using System.Web.Routing;

namespace aspdev.repaem
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("Content/{*pathInfo}");
			routes.IgnoreRoute("Scripts/{*pathInfo}");
			routes.IgnoreRoute("Images/{*pathInfo}");

			routes.IgnoreRoute("elmah.axd");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
				namespaces: new string[] {"aspdev.repaem.Controllers"}
				);
		}
	}
}