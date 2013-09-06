using System;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Models.Data
{
    public class RepaemPrincipal : IPrincipal
    {
        IUserService us;
        IIdentity ii;

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
        IUserService us;

        public RepaemIdentity(string username)
        {
            Name = username;
            us = DependencyResolver.Current.GetService<IUserService>();
        }

        public string AuthenticationType
        {
            get { return "Form"; }
        }

        public bool IsAuthenticated
        {
            get 
            {
                return us.IsAuth;
            }
        }

        public string Name
        {
            get;
            private set;
        }
    }
}