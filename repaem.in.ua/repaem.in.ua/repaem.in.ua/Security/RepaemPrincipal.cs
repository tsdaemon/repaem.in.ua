using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Security
{
	public class RepaemPrincipal : IPrincipal
	{
		private readonly IIdentity _ii;
		private readonly IUserService _us;

		public RepaemPrincipal(IIdentity ii)
		{
			_ii = ii;
			_us = DependencyResolver.Current.GetService<IUserService>();
		}

		public IIdentity Identity
		{
			get { return _ii; }
		}

		public bool IsInRole(string role)
		{
			return _us.UserIsInRole(role);
		}
	}

	public class RepaemIdentity : IIdentity
	{
		private readonly IUserService _us;

		public RepaemIdentity(string username)
		{
			Name = username;
			_us = DependencyResolver.Current.GetService<IUserService>();
		}

		public string AuthenticationType
		{
			get { return "Form"; }
		}

		public bool IsAuthenticated
		{
			get { return _us.CurrentUser != null; }
		}

		public string Name { get; private set; }
	}
}