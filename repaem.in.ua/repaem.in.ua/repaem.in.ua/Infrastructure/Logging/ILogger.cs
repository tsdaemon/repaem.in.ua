using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspdev.repaem.Infrastructure.Logging
{
	public interface ILogger
	{
		void Info(string message);

		void Trace(string message);

		void Error(string message);

		void Error(Exception e);
	}
}