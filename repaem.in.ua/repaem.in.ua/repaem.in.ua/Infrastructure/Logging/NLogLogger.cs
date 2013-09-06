using NLog;
using System;

namespace aspdev.repaem.Infrastructure.Logging
{
	public class NLogLogger : ILogger
	{
		private readonly Logger _log;

		public NLogLogger()
		{
			_log = LogManager.GetCurrentClassLogger();
		}

		public void Info(string message)
		{
			_log.Info(message);
		}

		public void Trace(string message)
		{
			_log.Trace(message);
		}

		public void Error(string message)
		{
			_log.Error(message);
		}

		public void Error(Exception e)
		{
			_log.Error("Unhandled exception: {2}, {0} in {1}", e.Message, e.StackTrace, e.GetType().Name);
		}
		
		public void Warn(Exception e)
		{
			_log.Warn("Unhandled exception: {2}, {0} in {1}", e.Message, e.StackTrace, e.GetType().Name);
		}
	}
}