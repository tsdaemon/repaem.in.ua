using System.Web.Http.Controllers;
using System.Web.Mvc;
using aspdev.repaem.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure
{
	public class RepaemAdditionalInfoAttribute : ActionFilterAttribute
	{
		public string Title { get; set; }

		public bool UnpaidBills { get; set; }

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var viewBag = filterContext.Controller.ViewBag;
			if (UnpaidBills)
			{
				var us = DependencyResolver.Current.GetService<IUserService>();
				viewBag.HaveUnpaidBill = us.HaveUnpaidBill;

			}
			if (!string.IsNullOrEmpty(Title))
				viewBag.Title = Title;
		}
	}
}