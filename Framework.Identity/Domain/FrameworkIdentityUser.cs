using Extenso.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace Framework.Identity.Domain
{
    public abstract class FrameworkIdentityUser : IdentityUser, IEntity
    {
        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}