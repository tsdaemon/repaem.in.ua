using System;

namespace aspdev.repaem.Infrastructure.Logging
{
	public interface ILogger
	{
		void Info(string message);

		void Trace(string message);

		void Error(string message);

		void Error(Exception e);

		void Warn(Exception e);
	}
}