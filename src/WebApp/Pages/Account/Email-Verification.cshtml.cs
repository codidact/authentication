using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Common.Interfaces;


namespace Codidact.Authentication.WebApp.Pages.Account
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class EmailVerificationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _emailService;
        public EmailVerificationModel(
            UserManager<ApplicationUser> userManager,
            IMailService emailService
        )
        {
            _userManager = userManager;
            _emailService = emailService;

        }
        [Required]
        [FromQuery(Name = "returnurl")]
        public string ReturnUrl { get; set; } = "/index";
        [FromQuery(Name = "token")]
        [Required(ErrorMessage = "Invalid email verification request")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Invalid email verification request")]
        [FromQuery(Name = "email")]
        public string Email { get; set; }
        public bool Verified { get; set; }
        public async Task<IActionResult> OnGet(string token, string returnUrl, string email)
        {
            if (!ModelState.IsValid)
            {
                return LocalRedirect("/Index");
            }
            Token = token;
            ReturnUrl = returnUrl;
            Email = email;
            return await ConfirmEmailAsync();
        }
        public async Task<IActionResult> OnPostEmailVerifyAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _emailService.SendVerificationEmail(user, token, "/index");
            return Content("Email-Sent");
        }
        public async Task<IActionResult> ConfirmEmailAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(HttpUtility.UrlDecode(Email));
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email not found");
                }
                else
                {
                    if (!user.EmailConfirmed)
                    {
                        var result = await _userManager.ConfirmEmailAsync(user, Token);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                            }
                            Verified = result.Succeeded;
                        }
                        else
                        {
                            Verified = result.Succeeded;
                        }
                    }
                    else
                    {
                        return LocalRedirect("/Index");
                    }
                }
            }
            return Page();
        }
    }
}

