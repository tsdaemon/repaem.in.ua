using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Services
{
    public interface IMessagesProvider
    {
        public void SendMessage(string message, string[] phones, string[] emails);

        public void SendMessage<T>(MessagesType type, T obj, string[] phones, string[] emails);
    }

    public class RepaemMessagesProvider : IMessagesProvider
    {
        IEmailSender _email;
        ISmsSender _sms;

        public RepaemMessagesProvider(IEmailSender email, ISmsSender sms)
        {
            _email = email;
            _sms = sms;
        }

        public void SendMessage(string message, string[] phones, string[] emails)
        {
            //TODO: fucking do this
        }


        public void SendMessage<T>(MessagesType type, T obj, string[] phones, string[] emails)
        {
 	        throw new NotImplementedException();
        }
    }

    public enum MessagesType
    {
        REP_IS_CANCELLED,
        REP_IS_CANCELLED_FIXED,
        REP_IS_CANCELLED_FIXED_ONE
    }
}