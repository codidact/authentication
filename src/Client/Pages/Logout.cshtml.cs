using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Codidact.Authentication.Client.Pages
{
    public class LogoutModel : PageModel
    {
        // Todo. Deal with return URLs.

        public IActionResult OnPost()
        {
            return SignOut("cookie", "oidc");
        }
    }
}
