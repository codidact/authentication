using System.ComponentModel.DataAnnotations;

namespace Codidact.Authentication.Web.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
