using System.Web.Mvc;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Controllers
{
	public class RepaemControllerBase : Controller
	{
		public RepaemControllerBase(RepaemLogicProvider lg)
		{
			Logic = lg;
		}

		protected void SetMessage(Message message)
		{
			TempData["Message"] = message;
		}

		protected RepaemLogicProvider Logic { get; set; }
	}
}