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
        void SendRegisteredMail(User u);

        void SendBookedMail(RepBaseBook rb, string email);

        void SendRepetitionIsCancelled(string Email, string RoomName, string RepBaseName, DateTime TimeStart, DateTime TimeEnd);
    }

    public class TestEmailSender : IEmailSender
    {
        public void SendRegisteredMail(User u)
        {
            
        }

        public void SendBookedMail(RepBaseBook rb, string email)
        {

        }

        public void SendRepetitionIsCancelled(string Email, string RoomName, string RepBaseName, DateTime TimeStart, DateTime TimeEnd)
        {

        }
    }
}