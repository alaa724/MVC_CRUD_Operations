using System.Threading.Tasks;

namespace Route.C41.G01.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendlAsync(string from, string recipents, string subject, string body);
	}
}
