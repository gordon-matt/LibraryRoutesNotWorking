using System.Collections.Generic;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace MainApp.Data.Domain
{
    public class ApplicationRole : IdentityRole, IEntity
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}