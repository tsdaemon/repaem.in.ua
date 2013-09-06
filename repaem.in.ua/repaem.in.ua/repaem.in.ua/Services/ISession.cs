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
		User User { get; set; }
	}

	public class HttpSession : ISession
	{
		private HttpSessionState _session {
			get { return HttpContext.Current.Session; }
		}

		public DateTime? BookDate
		{
			get
			{
				var ss = _session["BookDate"];
				if (ss != null)
					return (DateTime) ss;
				return null;
			}
			set { _session["BookDate"] = value; }
		}

		public TimeRange BookTime
		{
			get { return (TimeRange) _session["BookTime"]; }
			set { _session["BookTime"] = value; }
		}

		public int? BookBaseId
		{
			get
			{
				var ss = _session["BookBaseId"];
				if (ss != null)
					return (int) ss;
				return null;
			}
			set { _session["BookBaseId"] = value; }
		}

		public int? Capcha
		{
			get
			{
				var ss = _session["Capcha"];
				if (ss != null)
					return (int) ss;
				return null;
			}
			set { _session["Capcha"] = value; }
		}

		public int? Sms
		{
			get
			{
				var ss = _session["SMS"];
				if (ss != null)
					return (int) ss;
				return null;
			}
			set { _session["SMS"] = value; }
		}


		public RepBaseFilter Filter
		{
			get { return _session["Filter"] as RepBaseFilter; }
			set { _session["Filter"] = value; }
		}

		public User User
		{
			get
			{
				if (_session == null) return null;
				return _session["User"] as User; 
			}
			set { _session["User"] = value; }
		}

		public int? BookRoomId
		{
			get
			{
				var ss = _session["BookRoomId"];
				if (ss != null)
					return (int)ss;
				return null;
			}
			set { _session["BookRoomId"] = value; }
		}
	}
}