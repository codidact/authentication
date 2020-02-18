using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.Application.Services
{
    /// <summary>
    /// When the application is run for the first time, there are a number of things
    /// to take care of. This service does all of these things.
    /// </summary>
    public class InitializationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<InitializationService> _logger;

        public InitializationService(
            UserManager<ApplicationUser> userManager,
            ILogger<InitializationService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public Task<IdentityResult> CreateAdministratorAsync(string email, string password)
        {
            _logger.LogWarning($"Creating Administrator '{email}'.");

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
