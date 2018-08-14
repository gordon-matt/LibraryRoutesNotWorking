using Extenso.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace MainApp.Data.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IEntity
    {
        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}