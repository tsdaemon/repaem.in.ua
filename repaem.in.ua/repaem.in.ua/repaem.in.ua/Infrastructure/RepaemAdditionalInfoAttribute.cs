using System.Web.Mvc;
using aspdev.repaem.Security;

namespace aspdev.repaem.Infrastructure
{
	public class RepaemUnpaidBills : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var viewBag = filterContext.Controller.ViewBag;
			var us = DependencyResolver.Current.GetService<RepaemUserService>();
			viewBag.HaveUnpaidBill = us.HaveUnpaidBill;
		}
	}

	public class RepaemTitle : ActionFilterAttribute
	{
		public string Title { get; set; }

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (!string.IsNullOrEmpty(Title))
			{
				var viewBag = filterContext.Controller.ViewBag;
				var title = "repaem.in.ua - " + Title;

				if (viewBag.Title != null)
					title = title + " - " + viewBag.Title;

				viewBag.Title = title;
			}
		}
	}
}