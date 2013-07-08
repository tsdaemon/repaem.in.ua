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

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class RepApiControllerActionInvoker : ApiControllerActionInvoker
    {
        ILogger _log;

        public RepApiControllerActionInvoker() : base()
        {
            _log = DependencyResolver.Current.GetService<ILogger>();
        }

        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            _log.Info(String.Format("Request {0}, action {1}", actionContext.Request, actionContext.ActionDescriptor.ActionName));

            var result = base.InvokeActionAsync(actionContext, cancellationToken);

            if (result.Exception != null && result.Exception.GetBaseException() != null)
            {
                var baseException = result.Exception.GetBaseException();

                _log.Error(baseException);

                return Task.Run<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(""),
                    ReasonPhrase = "Critical Error"
                });
            }

            return result;
        }
    }

    public class RepControllerActionInvoker : ControllerActionInvoker
    {
        ILogger _log;

        public RepControllerActionInvoker()
            : base()
        {
            _log = DependencyResolver.Current.GetService<ILogger>();
        }

        public override bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            StringBuilder sb = new StringBuilder();
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
                throw new HttpException(500, "Internal error");
            }
        }
    }
}