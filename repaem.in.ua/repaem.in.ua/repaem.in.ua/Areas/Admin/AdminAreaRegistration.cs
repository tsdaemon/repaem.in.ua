using System.Web.Mvc;

namespace aspdev.repaem.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Admin"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Admin_default",
				"Admin/{controller}/{action}/{id}",
				new {controller = "RepBase", action = "List", id = UrlParameter.Optional},
				new[] {"aspdev.repaem.Areas.Admin.Controllers"}
				);
		}
	}
}