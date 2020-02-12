using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.Application.Services
{
    /// <summary>
    /// When the application is run for the first thme, there are a number of things
    /// to take care of. This service does all of these things.
    /// </summary>
    public class InitializationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public InitializationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> CreateAdministratorAsync(string email, string password)
        {
            return _userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = email,
                    Email = email
                }, password);
        }

        public bool DoesAdministratorExist()
        {
            return _userManager.Users.FirstOrDefault() != null;
        }
    }
}
