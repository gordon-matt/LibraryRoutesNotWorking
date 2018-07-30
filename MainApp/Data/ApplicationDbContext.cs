using MainApp.Data.Domain;
using Framework.Identity;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Data
{
    public class ApplicationDbContext : FrameworkIdentityDbContext<ApplicationUser, ApplicationRole>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}