using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using IdentityServer4.Events;
using IdentityServer4.Services;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Common.Interfaces;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _emailService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;
        public RegisterModel(SignInManager<ApplicationUser> signInManager,
                          UserManager<ApplicationUser> userManager,
                          IMailService emailService,
                          IEventService events)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _events = events;
        }

        [Required(ErrorMessage = "E-Mail Address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Display Name is required")]
        [DataType(DataType.Text)]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Confirmaton is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnUrl { get; set; } = "/index";
        public bool VerificationSent { get; set; }
        public void OnGet([FromQuery] string returnUrl)
        {
            if (returnUrl != null)
            {
                ReturnUrl = returnUrl;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Password.Equals(ConfirmPassword, System.StringComparison.InvariantCulture))
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Password Confirmation must match");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = Email,
                    UserName = Email,
                    DisplayName = DisplayName,
                };
                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _signInManager.PasswordSignInAsync(Email, Password, false, false);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));
                    await _emailService.SendVerificationEmail(user, token, ReturnUrl);
                    VerificationSent = true;
                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            return Page();
        }
    }
}
