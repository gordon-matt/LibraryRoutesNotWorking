using MainApp.Data;
using MainApp.Data.Domain;
using Framework.Identity;
using Microsoft.AspNetCore.Identity;

namespace MainApp.Identity
{
    public class ApplicationRoleStore : FrameworkRoleStore<ApplicationRole, ApplicationDbContext>
    {
        public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}