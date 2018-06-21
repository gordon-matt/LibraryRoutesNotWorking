using FrameworkDemo.Data;
using FrameworkDemo.Data.Domain;
using Framework.Identity;
using Microsoft.AspNetCore.Identity;

namespace FrameworkDemo.Identity
{
    public class ApplicationRoleStore : FrameworkRoleStore<ApplicationRole, ApplicationDbContext>
    {
        public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}