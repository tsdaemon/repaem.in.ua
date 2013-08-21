using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure;
using System.Web.Mvc;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	[RepaemAuth(Roles = "Manager")]
	[RepaemAdditionalInfoAttribute(UnpaidBills = true)]
	public class RepaemAdminControllerBase : Controller
	{
		protected IManagerLogicProvider Logic;

		public RepaemAdminControllerBase(IManagerLogicProvider logic)
			: base()
		{
			Logic = logic;
		}
	}
}