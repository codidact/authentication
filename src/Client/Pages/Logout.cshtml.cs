using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Codidact.Authentication.Client.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Todo. Is this a CSRF vulnerbility? The 'oidc' schema should be fine,
            // but the cookies are removed without a POST request, or not?
            return SignOut("cookie", "oidc");
        }
    }
}
