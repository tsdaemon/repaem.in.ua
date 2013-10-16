using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using aspdev.repaem.Services;

namespace repaemTest.Messages
{
	[TestClass]
	public class EmailTest
	{
		private EmailSender email;

		[TestInitialize]
		public void Init()
		{
			email = new EmailSender();
		}

		[TestMethod]
		public void TestSendMail()
		{
			email.SendEmail("tsdaemon@gmail.com", "123", "123");
		}
	}
}
