using MainApp.Data.Domain;
using Framework.Identity;
using Microsoft.AspNetCore.Identity;

namespace MainApp.Identity
{
    public class ApplicationRoleValidator : FrameworkRoleValidator<ApplicationRole>
    {
        public ApplicationRoleValidator(IdentityErrorDescriber errors = null)
            : base(errors)
        {
        }
    }
}