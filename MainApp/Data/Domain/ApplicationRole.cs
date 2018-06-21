using System.Collections.Generic;
using Framework.Identity.Domain;

namespace FrameworkDemo.Data.Domain
{
    public class ApplicationRole : FrameworkIdentityRole
    {
        public virtual ICollection<RolePermission> RolesPermissions { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}