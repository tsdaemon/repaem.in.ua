using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	[RepaemAuth(Roles = "Manager")]
	public class RepaemAdminControllerBase : Controller
	{
		protected IAdminLogicProvider _logic;

		public RepaemAdminControllerBase(IAdminLogicProvider logic)
			: base()
		{
			_logic = logic;
		}
	}
}