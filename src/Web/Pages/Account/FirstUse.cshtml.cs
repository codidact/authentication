using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Services;

namespace Codidact.Authentication.Web.Pages
{
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

        // Todo. Validate CSRF-token.
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
