using Microsoft.AspNetCore.Identity;

namespace Codidact.Authentication.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        /// <summary>
        /// Foreign key for the 'Member' table in the 'core' application.
        /// </summary>
        public int MemberId { get; set; }
    }
}
