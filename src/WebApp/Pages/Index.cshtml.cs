using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Common.Interfaces;

namespace Codidact.Authentication.WebApp.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(
                UserManager<ApplicationUser> userManager,
                IMailService emailService
                )
        {
            _userManager = userManager;
        }
        public bool EmailVerified { get; set; } = false;
        public async void OnGet()
        {
            if (User?.Identity.IsAuthenticated ?? true)
            {
                var user = await _userManager.GetUserAsync(User);
                EmailVerified = user.EmailConfirmed;
            }
        }
    }
}
