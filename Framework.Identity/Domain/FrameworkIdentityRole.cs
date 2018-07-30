using Extenso.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace Framework.Identity.Domain
{
    public abstract class FrameworkIdentityRole : IdentityRole, IEntity
    {
        public FrameworkIdentityRole()
            : base()
        {
        }

        public FrameworkIdentityRole(string roleName)
            : base(roleName)
        {
        }
        
        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}