using System.Web.Mvc;
using System.Web.Routing;

namespace aspdev.repaem
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.IgnoreRoute("elmah.axd");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
				namespaces: new string[] {"aspdev.repaem.Controllers"}
				);

			routes.MapRoute(
				name: "Search",
				url: "{controller}/{action}/{pattern}",
				defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
				namespaces: new string[] {"aspdev.repaem.Controllers"}
				);

			routes.MapRoute(
				name: "Book",
				url: "{controller}/{action}/{id}/{datetime}/{roomid}",
				defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
				namespaces: new string[] {"aspdev.repaem.Controllers"}
				);
		}
	}
}