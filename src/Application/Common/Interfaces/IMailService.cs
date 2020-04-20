using System.Threading.Tasks;
using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.Application.Common.Interfaces
{
    public interface IMailService
    {
        Task SendResetPassword(ApplicationUser user, string token, string returnUrl);
        Task SendVerificationEmail(ApplicationUser user, string token, string returnUrl);
    }
}
