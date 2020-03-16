using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
    [BindProperties]
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

        [Required]
        public string ReturnUrl { get; set; } = "/index";

        public void OnGet([FromQuery] string returnUrl)
        {
            if (returnUrl != null)
            {
                ReturnUrl = returnUrl;
            }
        }

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
