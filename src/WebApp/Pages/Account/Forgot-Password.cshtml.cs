using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Infrastructure.Common.Interfaces;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [BindProperties]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender<EmailSettings> _emailService;

        public ForgotPasswordModel(
                          UserManager<ApplicationUser> userManager,
                          IEmailSender<EmailSettings> emailService
            )
        {
            _userManager = userManager;
            _emailService = emailService;
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
                    await _emailService.SendResetPassword(Email, token, "http://localhost:8001/");
                    Sent = true;
                }
                else
                {
                    ModelState.AddModelError("Email", "No user found with this email");
                }
            }

            return Page();
        }
    }
}
