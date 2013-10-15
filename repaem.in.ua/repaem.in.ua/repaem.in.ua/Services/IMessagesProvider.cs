using System;

namespace aspdev.repaem.Services
{
	public interface IMessagesProvider
	{
		void SendMessage(string message, string[] phones, string[] emails);

		void SendMessage(string subject, string message, string[] phones, string[] emails);
	}

	public class RepaemMessagesProvider : IMessagesProvider
	{
		private IEmailSender _email;
		private ISmsSender _sms;

		public RepaemMessagesProvider(IEmailSender email, ISmsSender sms)
		{
			_email = email;
			_sms = sms;
		}

		public void SendMessage(string message, string[] phones, string[] emails)
		{
			//TODO: fucking do this
		}

		public void SendMessage(string subject, string message, string[] phones, string[] emails)
		{
			
		}
	}
}