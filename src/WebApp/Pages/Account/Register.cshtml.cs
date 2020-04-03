using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICoreApiService _coreApiService;

        public RegisterModel(
                          UserManager<ApplicationUser> userManager,
                          ICoreApiService coreApiService)
        {
            _userManager = userManager;
            _coreApiService = coreApiService;
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
                var applicationUser = new ApplicationUser
                {
                    Email = Email,
                    UserName = Email,
                    DisplayName = DisplayName,
                };
                var userResult = await _userManager.CreateAsync(applicationUser, Password);
                if (!userResult.Succeeded)
                {
                    foreach (var error in userResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                else
                {
                    var memberResult = await _coreApiService
                        .CreateMemberAsync(ReturnUrl, DisplayName, applicationUser.Id);
                    if (!memberResult.Success)
                    {
                        foreach (var error in userResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                    else
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                }

            }

            return Page();
        }
    }
}
