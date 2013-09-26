using System.Web.Security;
using aspdev.repaem.Models.Data;
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
		int? BookRoomId { get; set; } 

		int? Capcha { get; set; }

		int? Sms { get; set; }

		RepBaseFilter Filter { get; set; }
	}

	public class HttpSession : ISession
	{
		protected HttpSessionState Session {
			get { return HttpContext.Current.Session; }
		}

		public DateTime? BookDate
		{
			get
			{
				var ss = Session["BookDate"];
				if (ss != null)
					return (DateTime) ss;
				return null;
			}
			set { Session["BookDate"] = value; }
		}

		public TimeRange BookTime
		{
			get { return (TimeRange) Session["BookTime"]; }
			set { Session["BookTime"] = value; }
		}

		public int? BookBaseId
		{
			get
			{
				var ss = Session["BookBaseId"];
				if (ss != null)
					return (int) ss;
				return null;
			}
			set { Session["BookBaseId"] = value; }
		}

		public int? Capcha
		{
			get
			{
				var ss = Session["Capcha"];
				if (ss != null)
					return (int) ss;
				return null;
			}
			set { Session["Capcha"] = value; }
		}

		public int? Sms
		{
			get
			{
				var ss = Session["SMS"];
				if (ss != null)
					return (int) ss;
				return null;
			}
			set { Session["SMS"] = value; }
		}

		public RepBaseFilter Filter
		{
			get { return Session["Filter"] as RepBaseFilter; }
			set { Session["Filter"] = value; }
		}

		public virtual int? BookRoomId
		{
			get
			{
				var ss = Session["BookRoomId"];
				if (ss != null)
					return (int)ss;
				return null;
			}
			set { Session["BookRoomId"] = value; }
		}
	}

	public class DatabaseSession : HttpSession
	{
		private Database _db;

		public DatabaseSession(Database db)
		{
			_db = db;
		}

		public void SaveSessionInDb(string key)
		{
			var req = HttpContext.Current.Request;
			_db.SaveSession(key, 
				req.UserHostAddress, 
				req.UserHostName, 
				(req.UserLanguages.Length > 0 ? req.UserLanguages[0] : String.Empty), 
				req.UserAgent, 
				(req.UrlReferrer != null ? req.UrlReferrer.AbsoluteUri : String.Empty),
			  req.Browser.Browser);
		}
	}
}