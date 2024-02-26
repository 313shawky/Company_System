using CompanyG01.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace CompanyG01.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			// SMTP => Simple Mail Transfer Protocol
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true; // Encrypt Email
			Client.Credentials = new NetworkCredential("313shawky@gmail.com", "yaomltdjwovcptln\r\n");
			Client.Send("313shawky@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
