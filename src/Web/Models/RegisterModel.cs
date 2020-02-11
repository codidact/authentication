using System.ComponentModel.DataAnnotations;

namespace Codidact.Authentication.Web.Models
{
    public class RegisterModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // https://github.com/dotnet/aspnetcore/issues/4895
        public class PasswordsModel
        {
            [DataType(DataType.Password)]
            public string First { get; set; }

            [DataType(DataType.Password)]
            [Compare(nameof(First))]
            public string Second { get; set; }
        }

        public PasswordsModel Passwords { get; set; }
    }
}
