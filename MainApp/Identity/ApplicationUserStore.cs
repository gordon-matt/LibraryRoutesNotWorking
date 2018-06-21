using FrameworkDemo.Data;
using FrameworkDemo.Data.Domain;
using Framework.Identity;
using Framework.Identity.Domain;
using Microsoft.AspNetCore.Identity;

namespace FrameworkDemo.Identity
{
    public class ApplicationUserStore : ApplicationUserStore<ApplicationUser>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }

    public abstract class ApplicationUserStore<TUser> : FrameworkUserStore<TUser, ApplicationRole, ApplicationDbContext>
        where TUser : FrameworkIdentityUser, new()
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}