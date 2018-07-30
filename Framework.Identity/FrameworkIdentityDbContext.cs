using Framework.Data.Entity.EntityFramework;
using Framework.Identity.Domain;
using Framework.Infrastructure;
using Framework.Tenants.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Framework.Identity
{
    public abstract class FrameworkIdentityDbContext<TUser, TRole>
        : IdentityDbContext<TUser, TRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
        where TUser : FrameworkIdentityUser
        where TRole : FrameworkIdentityRole
    {
        #region Constructors

        public FrameworkIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        #endregion Constructors

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var configurations = EngineContext.Current.ResolveAll<IFrameworkEntityTypeConfiguration>();

            foreach (dynamic configuration in configurations)
            {
                modelBuilder.ApplyConfiguration(configuration);
            }
        }
    }
}