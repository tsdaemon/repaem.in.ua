using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.Services;

namespace aspdev.repaem.Areas.Admin.Services
{
	public interface IManagerLogicProvider
	{
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
	}
}