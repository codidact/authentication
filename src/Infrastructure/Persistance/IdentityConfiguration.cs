using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.Infrastructure.Persistance
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("identity_user");
            builder.Entity<IdentityRole<long>>().ToTable("identity_role");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("identity_role_claim");
            builder.Entity<IdentityUserLogin<long>>().ToTable("identity_user_login");
            builder.Entity<IdentityUserClaim<long>>().ToTable("identity_user_claim");
            builder.Entity<IdentityUserRole<long>>().ToTable("identity_user_role");
            builder.Entity<IdentityUserToken<long>>().ToTable("identity_user_token");
        }
    }
}
