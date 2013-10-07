using aspdev.repaem.Infrastructure.Logging;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

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
			catch (HttpException)
			{
				throw;
			}
			catch (RepaemNotFoundException e)
			{
				throw new HttpException(404, e.Message);
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