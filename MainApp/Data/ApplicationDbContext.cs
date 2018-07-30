using MainApp.Data.Domain;
using Framework.Identity;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Data
{
    public class ApplicationDbContext : FrameworkIdentityDbContext<ApplicationUser, ApplicationRole>
    {
        public DbSet<UserProfileEntry> UserProfiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}