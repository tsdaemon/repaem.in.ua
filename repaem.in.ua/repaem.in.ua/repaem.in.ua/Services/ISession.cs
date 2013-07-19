using aspdev.repaem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace aspdev.repaem.Models.Data
{
    public interface ISession
    {
        DateTime? BookDate { get; set; }
        TimeRange BookTime { get; set; }
        int? BookBaseId { get; set; }

        int? Capcha { get; set; }

        int? Sms { get; set; }
    }

    public class HttpSession : ISession
    {
        HttpSessionState session = HttpContext.Current.Session;

        public DateTime? BookDate
        {
            get
            {
                object ss = session["BookDate"];
                if (ss != null)
                    return (DateTime)ss;
                else
                    return null;
            }
            set
            {
                session["BookDate"] = value;
            }
        }

        public TimeRange BookTime
        {
            get
            {
                return (TimeRange)session["BookTime"];
            }
            set
            {
                session["BookTime"] = value;
            }
        }

        public int? BookBaseId
        {
            get
            {
                object ss = session["BookBaseId"];
                if (ss != null)
                    return (int)ss;
                else
                    return null;
            }
            set
            {
                session["BookBaseId"] = value;
            }
        }

        public int? Capcha
        {
            get
            {
                object ss = session["Capcha"];
                if (ss != null)
                    return (int)ss;
                else
                    return null;
            }
            set
            {
                session["Capcha"] = value;
            }
        }

        public int? Sms
        {
            get
            {
                object ss = session["SMS"];
                if (ss != null)
                    return (int)ss;
                else
                    return null;
            }
            set
            {
                session["SMS"] = value;
            }
        }
    }
}
