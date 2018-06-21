using Framework.Tenants.Domain;
using Microsoft.AspNetCore.Identity;

namespace Framework.Identity.Domain
{
    public abstract class FrameworkIdentityUser : IdentityUser, ITenantEntity
    {
        public int? TenantId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}