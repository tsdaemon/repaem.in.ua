using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using aspdev.repaem.Models.Data;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Services
{
  //Логирование для этого
  public interface IEmailSender
  {
	  void SendEmail(string to, string title, string text);
  }

  public class TestEmailSender : IEmailSender
  {
		public void SendEmail(string to, string title, string text)
		{
			
		}
	}

	public class EmailSender : IEmailSender
	{
		public void SendEmail(string to, string subject, string text)
		{
			MailMessage mail = new MailMessage();
			mail.To.Add(new MailAddress(to));
			mail.Subject = subject;
			mail.Body = text;
			SmtpClient smtp = new SmtpClient();
			smtp.Send(mail);
		}
	}
}