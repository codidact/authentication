using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

using IdentityServer4.Services;
using IdentityServer4.Events;
using IdentityServer4.Extensions;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.WebApp.Common;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [SecurityHeaders]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;

        public LogoutModel(
            SignInManager<ApplicationUser> signInManager,
            IEventService events)
        {
            _signInManager = signInManager;
            _events = events;
        }

        public string ReturnUrl { get; set; } = "/index";

        public async Task<IActionResult> OnPostAsync()
        {
            if (User?.Identity.IsAuthenticated ?? false)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            return Redirect(ReturnUrl);
        }
    }
}
