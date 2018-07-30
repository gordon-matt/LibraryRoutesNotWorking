using System.Collections.Generic;
using System.Linq;
using Extenso.Data.Entity;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Tenants.Domain;

namespace Framework.Web.Infrastructure
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            EnsureTenant();

            var tenantRepository = EngineContext.Current.Resolve<IRepository<Tenant>>();
            IEnumerable<int> tenantIds = null;

            using (var connection = tenantRepository.OpenConnection())
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
            var tenantRepository = EngineContext.Current.Resolve<IRepository<Tenant>>();

            if (tenantRepository.Count() == 0)
            {
                tenantRepository.Insert(new Tenant
                {
                    Name = "Default",
                    Url = "my-domain.com",
                    Hosts = "my-domain.com"
                });
            }
        }
    }
}