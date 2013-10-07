using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Griffin.MvcContrib.Localization;
using aspdev.repaem.App_GlobalResources;
using aspdev.repaem.Security;
using aspdev.repaem.ViewModel;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Services;

namespace aspdev.repaem
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : HttpApplication
	{
		private const string COOKIE_KEY = "ASP.NET_SessionId";

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			//Есть несколько особых случаев при работе с регистрацией, так что пришлось сделать свой биндер
			ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof (Register), new RegisterBinder()));
			ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof (double), new DoubleModelBinder()));

			//Для названий таблиц в множественном числе
			DapperExtensions.DapperExtensions.DefaultMapper = typeof (CustomPluralizedMapper<>);

			//base.BeginRequest += MvcApplication_BeginRequest;
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			if (Context.Session != null && Context.Session.IsNewSession && SessionKey != null)
			{
				Session.Timeout = 8000;
				var session = DependencyResolver.Current.GetService<DatabaseSession>();
				session.SaveSessionInDb(SessionKey);
			}
		}

		protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
		{
			if (FormsAuthentication.CookiesSupported == true)
			{
				if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
				{
					var username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
					var service = DependencyResolver.Current.GetService<RepaemUserService>();
					service.SetUser(username);
					var u = service.CurrentUser;

					//Let us set the Pricipal with our user specific details
					e.User = new System.Security.Principal.GenericPrincipal(
						new System.Security.Principal.GenericIdentity(u.Name, "Forms"), u.Role.Split(','));
				}
			}
		}

		public static string SessionKey
		{
			get
			{
				var e = HttpContext.Current.Request.Cookies[COOKIE_KEY];
				return e != null ? e.Value : null;
			}
		}
	}

	public class DoubleModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (result != null && !string.IsNullOrEmpty(result.AttemptedValue))
			{
				if (bindingContext.ModelType == typeof(double))
				{
					double temp;
					var attempted = result.AttemptedValue.Replace(",", ".");
					if (double.TryParse(
							attempted,
							NumberStyles.Number,
							CultureInfo.InvariantCulture,
							out temp)
					)
					{
						return temp;
					}
				}
			}
			return base.BindModel(controllerContext, bindingContext);
		}
	}

	public class RegisterBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var o = base.BindModel(controllerContext, bindingContext);
			var re = o as Register;
			if (re.Capcha == null)
			{
				re.Name = controllerContext.HttpContext.Request["Register.Name"];
				re.Email = controllerContext.HttpContext.Request["Register.Email"];
				re.Password = controllerContext.HttpContext.Request["Register.Password"];
				re.Password2 = controllerContext.HttpContext.Request["Register.Password2"];
				re.Phone = controllerContext.HttpContext.Request["Register.Phone"];
				re.CityId = int.Parse(controllerContext.HttpContext.Request["Register.CityId"]);
				re.Capcha.Value = int.Parse(controllerContext.HttpContext.Request["Register.Capcha.Value"]);
			}
			return re;
		}
	}
}