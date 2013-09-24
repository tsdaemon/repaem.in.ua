using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure;
using System.Web.Mvc;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	[RepaemAuth(Roles = "Manager")]
	[RepaemUnpaidBills]
	public class RepaemAdminControllerBase : Controller
	{
		protected RepaemManagerLogicProvider Logic;

		public RepaemAdminControllerBase(RepaemManagerLogicProvider logic)
			: base()
		{
			Logic = logic;
		}
	}
}