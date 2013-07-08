using aspdev.repaem.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace aspdev.repaem.Infrastructure.Exceptions
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute, System.Web.Mvc.IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            DependencyResolver.Current.GetService<ILogger>().Error(filterContext.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Произошла ошибка!"),
                ReasonPhrase = "Что-то не так. Обратитесь к Администратору сервиса"
            });
        }
    }
}