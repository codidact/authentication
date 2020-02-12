using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

using Codidact.Authentication.Infrastructure.Identity;

namespace Codidact.Authentication.Web.Models
{
    [BindProperties]
    public class FirstUseModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FirstUseModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        public IActionResult OnGet()
        {
            if (DidFirstUseAlreadyRun())
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (DidFirstUseAlreadyRun())
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = Email,
                        Email = Email
                    },
                    Password);

                if (result.Succeeded)
                {
                    return Redirect("/index");
                }
            }

            return Page();
        }

        private bool DidFirstUseAlreadyRun()
        {
            // Todo. Race condition when multiple clients open '/account/firstuse'.
            return _userManager.Users.FirstOrDefault() != null;
        }
    }
}
