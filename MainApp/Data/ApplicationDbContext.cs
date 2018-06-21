using FrameworkDemo.Data.Domain;
using Framework.Identity;
using Microsoft.EntityFrameworkCore;

namespace FrameworkDemo.Data
{
    public class ApplicationDbContext : FrameworkIdentityDbContext<ApplicationUser, ApplicationRole>
    {
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserProfileEntry> UserProfiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}