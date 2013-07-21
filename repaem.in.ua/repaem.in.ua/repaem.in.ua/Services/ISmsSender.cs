using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Services
{
    //Обязательно логи для этого
    public interface ISmsSender
    {
        /// <summary>
        /// Генерирует код проверки и шлет его на мобильный
        /// </summary>
        void SendCodeSms(string number);

        /// <summary>
        /// Генерирует и отсылает сообщение для хозяина базы о том, что заказана репетиция
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="roomName"></param>
        /// <param name="phoneNumber"></param>
        void SendRepetitionIsBooked(RepBaseBook rb, string roomName, string phoneNumber);
    }

    public class TestSmsSender : ISmsSender
    {
        ISession ss;

        public TestSmsSender(ISession _ss)
        {
            ss = _ss;
        }

        public void SendCodeSms(string number)
        {
            ss.Sms = 123;
        }

        public void SendRepetitionIsBooked(RepBaseBook rb, string roomName, string phoneNumber)
        {
            
        }
    }
}
