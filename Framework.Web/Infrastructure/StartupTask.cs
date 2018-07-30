using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Tenants.Domain;
using Framework.Tenants.Services;

namespace Framework.Web.Infrastructure
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            EnsureTenant();

            var tenantService = EngineContext.Current.Resolve<ITenantService>();
            IEnumerable<int> tenantIds = null;

            using (var connection = tenantService.OpenConnection())
            {
                tenantIds = connection.Query().Select(x => x.Id).ToList();
            }

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members

        private static void EnsureTenant()
        {
            var tenantService = EngineContext.Current.Resolve<ITenantService>();

            if (tenantService.Count() == 0)
            {
                tenantService.Insert(new Tenant
                {
                    Name = "Default",
                    Url = "my-domain.com",
                    Hosts = "my-domain.com"
                });
            }
        }
    }
}