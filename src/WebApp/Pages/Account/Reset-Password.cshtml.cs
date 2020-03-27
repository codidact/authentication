using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Codidact.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Codidact.Authentication.WebApp.Pages.Account
{

    [BindProperties]
    public class ResetPasswordModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(
                          UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
        }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Confirmaton is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnUrl { get; set; } = "/index";

        [Required(ErrorMessage = "Invalid reset password form")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Invalid reset password form")]
        public string Email { get; set; }

        public void OnGet([FromQuery] string token, [FromQuery]string returnUrl, [FromQuery]string email)
        {
            Token = token;
            ReturnUrl = returnUrl;
            Email = email;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password.Equals(ConfirmPassword, System.StringComparison.InvariantCulture))
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Password Confirmation must match");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email not found");
                }
                else
                {
                    var result = await _userManager.ResetPasswordAsync(user, Token, Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                    else
                    {
                        return RedirectToPage("Login");
                    }
                }
            }

            return Page();
        }
    }
}