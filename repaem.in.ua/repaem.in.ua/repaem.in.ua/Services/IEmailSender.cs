using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Services
{
    //Логирование для этого
    public interface IEmailSender
    {
        void SendRegisteMail(User u);
        void SendBookedMail(RepBaseBook rb, string[] emails);
    }

    public class TestEmailSender : IEmailSender
    {
        public void SendRegisteMail(User u)
        {
            
        }

        public void SendBookedMail(RepBaseBook rb, string[] emails)
        {
        }
    }
}