using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Security;

namespace aspdev.repaem.Models.Data
{
	public abstract class BaseProtectedRepo<T> : BaseRepo<T>, IProtected<T> where T : class, new()
	{
		protected readonly IUserService _us;

		protected BaseProtectedRepo(IUserService us)
		{
			_us = us;
		} 

		public override void Update(T t)
		{
			if (CheckAccess(t, _us.CurrentUser))
				base.Update(t);
			else 
				throw new RepaemAccessDeniedException();
		}

		public override void Delete(int d)
		{
			var t = Get(d);
			if (CheckAccess(t, _us.CurrentUser))
				base.Delete(d);
			else
				throw new RepaemAccessDeniedException();
		}

		public override void Delete(T t)
		{
			if (CheckAccess(t, _us.CurrentUser))
				base.Delete(t);
			else
				throw new RepaemAccessDeniedException();
		}

		public abstract bool CheckAccess(T t, User u);
	}
}