using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspdev.repaem.Services
{
    //Обязательно логи для этого
    public interface ISmsSender
    {
        void SendSms();
    }

    public class TestSmsSender : ISmsSender
    {
        ISession ss;

        public TestSmsSender(ISession _ss)
        {
            ss = _ss;
        }

        public void SendSms()
        {
            ss.Sms = 123;
        }
    }
}
