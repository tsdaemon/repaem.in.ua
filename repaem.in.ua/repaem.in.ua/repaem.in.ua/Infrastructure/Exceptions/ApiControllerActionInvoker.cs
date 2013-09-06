using aspdev.repaem.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Infrastructure.Exceptions
{
	public class RepControllerActionInvoker : ControllerActionInvoker
	{
		readonly ILogger _log;

		public RepControllerActionInvoker()
			: base()
		{
			_log = DependencyResolver.Current.GetService<ILogger>();
		}

		public override bool InvokeAction(ControllerContext controllerContext, string actionName)
		{
			var sb = new StringBuilder();
			sb.Append(String.Format("{0} {1}, agent: {2}, host {3}",
					controllerContext.HttpContext.Request.HttpMethod,
					controllerContext.HttpContext.Request.RawUrl,
					controllerContext.HttpContext.Request.UserAgent,
					controllerContext.HttpContext.Request.UserHostAddress));
			_log.Info(sb.ToString());

			try
			{
				return base.InvokeAction(controllerContext, actionName);
			}
			catch (Exception e)
			{
				_log.Error(e);
#if DEBUG
				throw;
#else
				var re = new HttpException(500, "Internal error");
				throw re;
#endif
			}
		}
	}
}