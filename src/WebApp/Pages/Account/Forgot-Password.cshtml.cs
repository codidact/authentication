using Codidact.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [BindProperties]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordModel(
                          UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
        }

        [Required(ErrorMessage = "E-Mail Address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool Sent { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // TODO: Send Email with the token
                    // _emailService.SendResetPassowrd(Email, result);
                }
                Sent = true;
            }

            return Page();
        }
    }
}