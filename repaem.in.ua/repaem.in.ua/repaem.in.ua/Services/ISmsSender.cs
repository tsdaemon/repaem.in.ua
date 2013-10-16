using System.Configuration;
using SMSProject.Services;
using aspdev.repaem.Infrastructure.Exceptions;

namespace aspdev.repaem.Services
{
	//Обязательно логи для этого
	public interface ISmsSender
	{
		void SendSms(string number, string text);
	}

	public class TestSmsSender : ISmsSender
	{
		public void SendSms(string number, string text)
		{
		}
	}
	public class TurboSmsSender : ISmsSender
	{
		private string name;
		private string login;
		private string password;
		private SMSWorker service;
		private const string AUTH_SUCCESS = "Вы успешно авторизировались";
		private const string SEND_SUCCESS = "Сообщения успешно отправлены";

		public TurboSmsSender()
		{
			name = ConfigurationManager.AppSettings["TurboSms.Name"];
			login = ConfigurationManager.AppSettings["TurboSms.Login"];
			password = ConfigurationManager.AppSettings["TurboSms.Password"];

			service = SMSWorker.GetInstance();
		}

		public void SendSms(string number, string text)
		{
			string message = service.Auth(login, password);
			if(AUTH_SUCCESS != message)
				throw new RepaemSmsException(message);

			message = service.SendSMS(name, number, text, string.Empty)[0];
			if (SEND_SUCCESS != message)
				throw new RepaemSmsException(message);
		}
	}
}