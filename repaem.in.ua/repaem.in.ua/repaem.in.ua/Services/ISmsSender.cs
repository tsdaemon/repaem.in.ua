namespace aspdev.repaem.Services
{
	//Обязательно логи для этого
	public interface ISmsSender
	{
		/// <summary>
		///   Генерирует код проверки и шлет его на мобильный
		/// </summary>
		void SendCodeSms(string number);
	}

	public class TestSmsSender : ISmsSender
	{
		private ISession ss;

		public TestSmsSender(ISession _ss)
		{
			ss = _ss;
		}

		public void SendCodeSms(string number)
		{
			ss.Sms = 123;
		}
	}
}