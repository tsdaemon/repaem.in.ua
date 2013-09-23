using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Models
{
	public interface IProtected<in T>
	{
		bool CheckAccess(T t, User u);
	}
}