using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Areas.Admin.Services
{
	public interface IAdminLogicProvider
	{
		HomePage GetHomePage();
	}

	public class RepaemAdminLogicProvider : IAdminLogicProvider
	{
		private ISession _ss;
		private IEmailSender _email;
		private ILogger _log;
		private ISmsSender _sms;
		private readonly IDatabase _db;
		private readonly IUserService _us;

		public RepaemAdminLogicProvider(ISession ss, IEmailSender email, ILogger log, ISmsSender sms, IDatabase db, IUserService us)
		{
			_ss = ss;
			_email = email;
			_log = log;
			_sms = sms;
			_db = db;
			_us = us;
		}

		public HomePage GetHomePage()
		{
			var userId = _us.CurrentUser.Id;
			var hm = new HomePage
				{
					Map = {Coordinates = _db.GetBasesCoordinatesByManager(userId)},
					NewRepetitions = _db.GetNewRepetitionsByManager(userId),
					RepBases = _db.GetRepBasesByManager(userId),
					UserName = _us.CurrentUser.Name
				};
			return hm;
		}
	}
}