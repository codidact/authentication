using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Codidact.Authentication.Web.Models
{
    public class RegisterModel : PageModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // https://github.com/dotnet/aspnetcore/issues/4895
        public class PasswordsModel
        {
            [Required, DataType(DataType.Password)]
            public string First { get; set; }

            [Required, DataType(DataType.Password)]
            [Compare(nameof(First))]
            public string Second { get; set; }
        }

        public PasswordsModel Passwords { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Todo.
                throw new NotImplementedException();
            }

            return Page();
        }
    }
}
