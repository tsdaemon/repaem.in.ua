using aspdev.repaem.ViewModel;
using System;
using System.Web;
using System.Web.SessionState;

namespace aspdev.repaem.Services
{
	public interface ISession
	{
		DateTime? BookDate { get; set; }
		TimeRange BookTime { get; set; }
		int? BookBaseId { get; set; }

		int? Capcha { get; set; }

		int? Sms { get; set; }

		RepBaseFilter Filter { get; set; }
	}
	//TODO: хранить сессию в базе, http сессия как кэш
	public class HttpSession : ISession
	{
		private HttpSessionState session = HttpContext.Current.Session;

		public DateTime? BookDate
		{
			get
			{
				object ss = session["BookDate"];
				if (ss != null)
					return (DateTime) ss;
				else
					return null;
			}
			set { session["BookDate"] = value; }
		}

		public TimeRange BookTime
		{
			get { return (TimeRange) session["BookTime"]; }
			set { session["BookTime"] = value; }
		}

		public int? BookBaseId
		{
			get
			{
				object ss = session["BookBaseId"];
				if (ss != null)
					return (int) ss;
				else
					return null;
			}
			set { session["BookBaseId"] = value; }
		}

		public int? Capcha
		{
			get
			{
				object ss = session["Capcha"];
				if (ss != null)
					return (int) ss;
				else
					return null;
			}
			set { session["Capcha"] = value; }
		}

		public int? Sms
		{
			get
			{
				object ss = session["SMS"];
				if (ss != null)
					return (int) ss;
				else
					return null;
			}
			set { session["SMS"] = value; }
		}


		public RepBaseFilter Filter
		{
			get { return session["Filter"] as RepBaseFilter; }
			set { session["Filter"] = value; }
		}
	}
}