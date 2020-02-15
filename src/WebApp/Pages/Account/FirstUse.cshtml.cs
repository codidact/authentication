using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Codidact.Authentication.Application.Services;
using Codidact.Authentication.WebApp.Common;

namespace Codidact.Authentication.WebApp.Pages.Account
{
    [SecurityHeaders]
    [BindProperties]
    public class FirstUseModel : PageModel
    {
        private readonly InitializationService _init;

        public FirstUseModel(InitializationService init)
        {
            _init = init;
        }

        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        public IActionResult OnGet()
        {
            if (_init.DoesAdministratorExist())
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_init.DoesAdministratorExist())
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var result = await _init.CreateAdministratorAsync(Email, Password);

                if (result.Succeeded)
                {
                    return Redirect("/index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Page();
        }
    }
}
