using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

using IdentityServer4.Events;
using IdentityServer4.Services;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.WebApp.Common;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [SecurityHeaders]
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventService _events;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
                          UserManager<ApplicationUser> userManager,
                          IEventService events)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _events = events;
        }

        [Required(ErrorMessage = "E-Mail Address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool RememberLogin { get; set; } = false;

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
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberLogin, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Email);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));

                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid credentials.");
                }
            }

            return Page();
        }
    }
}
