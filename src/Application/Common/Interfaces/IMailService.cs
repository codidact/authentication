using System.Threading.Tasks;
using Codidact.Authentication.Domain.Entities;


namespace Codidact.Authentication.Application.Common.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(ApplicationUser user, string subject, string message);
        Task SendResetPassword(ApplicationUser user, string token, string returnUrl);
    }
}
