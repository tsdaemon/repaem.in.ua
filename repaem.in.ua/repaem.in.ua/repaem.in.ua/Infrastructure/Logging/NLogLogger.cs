using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure.Logging
{
    public class NLogLogger : ILogger
    {
        private Logger _log;

        public NLogLogger()
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            _log.Info(message);
        }


        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Error(Exception e)
        {
            _log.Error("Unhandled exception: {0} in {1}", e.Message, e.StackTrace);
            //if (e.InnerException != null)
            //{
            //    _log.Error("Have inner exception!"
            //}
        }
    }
}