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
	public class SmsTest
	{
		private TurboSmsSender tsms;

		[TestInitialize]
		public void Init()
		{
			tsms = new TurboSmsSender();
		}

		//[TestMethod] что бы случайно не попал
		public void TestTurboSend()
		{
			tsms.SendSms("+380956956757", "Привет!");
		}
	}
}
