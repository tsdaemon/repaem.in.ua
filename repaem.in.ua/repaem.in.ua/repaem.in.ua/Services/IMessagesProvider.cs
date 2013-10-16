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
			foreach (var email in emails)
			{
				_email.SendEmail(email, String.Empty, message);
			}
			foreach (var phone in phones)
			{
				_sms.SendSms(phone, message);
			}
		}

		public void SendMessage(string subject, string message, string[] phones, string[] emails)
		{
			foreach (var email in emails)
			{
				_email.SendEmail(email, String.Empty, message);
			}
			foreach (var phone in phones)
			{
				_sms.SendSms(phone, message);
			}
		}
	}
}