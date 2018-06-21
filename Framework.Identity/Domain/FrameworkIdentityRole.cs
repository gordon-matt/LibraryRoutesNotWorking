using Framework.Tenants.Domain;
using Microsoft.AspNetCore.Identity;

namespace Framework.Identity.Domain
{
    public abstract class FrameworkIdentityRole : IdentityRole, ITenantEntity
    {
        public FrameworkIdentityRole()
            : base()
        {
        }

        public FrameworkIdentityRole(string roleName)
            : base(roleName)
        {
        }

        public int? TenantId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}