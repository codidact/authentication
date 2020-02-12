using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Codidact.Authentication.Infrastructure.Identity;

namespace Codidact.Authentication.Web.Models
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
                          ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        // Todo. Hook this property up with the page.
        public bool RememberLogin { get; set; } = false;

        // Todo. Hook this property up with the page.
        public string ReturnUrl { get; set; } = "/index";

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberLogin, false);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        // Todo. Do the Identity Server stuff.

                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        _logger.LogInformation($"Ignored login attempt with malicious return URL '{ReturnUrl}'.");
                        return BadRequest();
                    }
                }
                else
                {
                    // Todo. Lookup a localized string.
                    ModelState.AddModelError(string.Empty, "Invalid credentials.");
                }
            }

            return Page();
        }
    }
}
