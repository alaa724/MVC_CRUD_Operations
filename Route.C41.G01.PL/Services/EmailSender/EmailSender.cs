using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendAsync(string from, string recipents, string subject, string body)
		{
			var senderEmail = _configuration["EmailSettings:SenderEmail"];
			var senderpassword = _configuration["EmailSettings:SenderPassword"];

			var emailMassege = new MailMessage();
			emailMassege.From = new MailAddress(from);
			emailMassege.To.Add(recipents);
			emailMassege.Subject = subject;
			emailMassege.Body = $"<html><body>{body}</body></html>";
			emailMassege.IsBodyHtml = true;


			var smtpClient = new SmtpClient("", 587)
			{
				Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpClientServer"], _configuration["EmailSettings:SmtpClientPort"]),
				EnableSsl = true
			};

			await smtpClient.SendMailAsync(emailMassege);

		}

	}
}
