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
        DateTime BookDate { get; set; }
        TimeRange BookTime { get; set; }
        int BookBaseId { get; set; }

        int Capcha { get; set; }
    }

    public class HttpSession : ISession
    {
        HttpSessionState session = HttpContext.Current.Session;

        public DateTime BookDate
        {
            get
            {
                return (DateTime)session["BookDate"];
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

        public int BookBaseId
        {
            get
            {
                return (int)session["BookBaseId"];
            }
            set
            {
                session["BookBaseId"] = value;
            }
        }

        public int Capcha
        {
            get
            {
                return (int)session["Capcha"];
            }
            set
            {
                session["Capcha"] = value;
            }
        }
    }
}
