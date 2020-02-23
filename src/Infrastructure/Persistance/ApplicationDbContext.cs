using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.Infrastructure.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
