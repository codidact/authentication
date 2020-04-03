using System.Threading.Tasks;

namespace Codidact.Authentication.Infrastructure.Common.Interfaces
{
    public interface IEmailSender<EmailSettings>
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendResetPassword(string email, string token, string returnUrl);
    }
}
