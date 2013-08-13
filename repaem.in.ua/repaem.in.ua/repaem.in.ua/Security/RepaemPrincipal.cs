using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Security
{
	public class RepaemPrincipal : IPrincipal
	{
		private IIdentity ii;
		private IUserService us;

		public RepaemPrincipal(IIdentity _ii)
		{
			ii = _ii;
			us = DependencyResolver.Current.GetService<IUserService>();
		}

		public IIdentity Identity
		{
			get { return ii; }
		}

		public bool IsInRole(string role)
		{
			return us.UserIsInRole(ii.Name, role);
		}
	}

	public class RepaemIdentity : IIdentity
	{
		public RepaemIdentity(string username)
		{
			Name = username;
		}

		public string AuthenticationType
		{
			get { return "Form"; }
		}

		public bool IsAuthenticated
		{
			get
			{
				HttpCookie c = HttpContext.Current.Request.Cookies.Get("user_session");
				if (c != null)
				{
					return HttpContext.Current.Cache.Get(c.Value) != null;
				}
				return false;
			}
		}

		public string Name { get; private set; }
	}
}