using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Reflection;

namespace SMSProject.Services
{
    /// <summary>
    ///     ����� ��� ������ � ��������
    /// </summary>
    public class SMSWorker : Service
    {
        // ���� ������ ��� �������� �������� �������
        private const string SessionKey = "SMSWorker_Instance";

        class SMSProxy
        {
            public string Host;
            public int Port;
        }

        // �����������
        public SMSWorker()
        {
            this.CookieContainer = new CookieContainer();

            // ���� ������������ ������
            if (ConfigurationManager.AppSettings["UseProxy"] == "true")
            {
                // ���������� ��������� ������ �� Web.Config - �
                SMSProxy pr = new SMSProxy();
                pr.Host = ConfigurationManager.AppSettings["ProxyHost"];
                pr.Port = Convert.ToInt32(ConfigurationManager.AppSettings["ProxyPort"]);

                WebProxy proxy = new WebProxy(pr.Host, pr.Port);
                this.Proxy = proxy;
            }
        }

        public static SMSWorker GetInstance()
        {
            if (HttpContext.Current == null)
                return new SMSWorker();

            // ����������, ���������� �� ������ � ������
            SMSWorker worker = HttpContext.Current.Session[SessionKey] as SMSWorker;
            if (worker == null)
            {
                // ������� ������
                worker = new SMSWorker();
                // �������� � ������
                HttpContext.Current.Session[SessionKey] = worker;
            }
            return worker;
        }

        /// <summary>
        ///    �������� ������ � ������ (���� �����)
        /// </summary>
        public void CloseSession()
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Session[SessionKey] = null;
        }
    }
}
