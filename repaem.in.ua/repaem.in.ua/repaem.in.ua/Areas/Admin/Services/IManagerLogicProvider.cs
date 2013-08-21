using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.Services;

namespace aspdev.repaem.Areas.Admin.Services
{
	public interface IManagerLogicProvider
	{
		HomeIndex GetHomeIndex();
	}

	public class RepaemManagerLogicProvider : IManagerLogicProvider
	{
		private ISession _ss;
		private IEmailSender _email;
		private ILogger _log;
		private ISmsSender _sms;
		private readonly IDatabase _db;
		private readonly IUserService _us;

		public RepaemManagerLogicProvider(ISession ss, IEmailSender email, ILogger log, ISmsSender sms, IDatabase db, IUserService us)
		{
			_ss = ss;
			_email = email;
			_log = log;
			_sms = sms;
			_db = db;
			_us = us;
		}

		public HomeIndex GetHomeIndex()
		{
			var hm = new HomeIndex
				{
					Comments = _db.GetCommentsByManager(_us.CurrentUser.Id),
					NewRepetitions = _db.GetNewRepetitionsByManager(_us.CurrentUser.Id)
				};
			return hm;
		}
	}
}