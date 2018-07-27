using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extenso;
using Extenso.Collections;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Tenants.Domain;
using Framework.Tenants.Services;
using Framework.Threading;
using Framework.Web.Security.Membership.Permissions;

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

            AsyncHelper.RunSync(() => EnsurePermissions(membershipService, tenantIds));
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

        private static async Task EnsurePermissions(IMembershipService membershipService, IEnumerable<int> tenantIds)
        {
            if (membershipService.SupportsRolePermissions)
            {
                #region NULL Tenant

                var permissionProviders = EngineContext.Current.ResolveAll<IPermissionProvider>();

                var allPermissions = permissionProviders.SelectMany(x => x.GetPermissions());
                var allPermissionNames = allPermissions.Select(x => x.Name).ToHashSet();

                var installedPermissions = await membershipService.GetAllPermissions(null);
                var installedPermissionNames = installedPermissions.Select(x => x.Name).ToHashSet();

                var permissionsToAdd = allPermissions
                    .Where(x => !installedPermissionNames.Contains(x.Name))
                    .Select(x => new FrameworkPermission
                    {
                        Name = x.Name,
                        TenantId = null,
                        Category = x.Category,
                        Description = x.Description
                    })
                    .OrderBy(x => x.Category)
                    .ThenBy(x => x.Name);

                if (!permissionsToAdd.IsNullOrEmpty())
                {
                    await membershipService.InsertPermissions(permissionsToAdd);
                }

                var permissionIdsToDelete = installedPermissions
                    .Where(x => !allPermissionNames.Contains(x.Name))
                    .Select(x => x.Id);

                if (!permissionIdsToDelete.IsNullOrEmpty())
                {
                    await membershipService.DeletePermissions(permissionIdsToDelete);
                }

                #endregion NULL Tenant

                #region Tenants

                foreach (int tenantId in tenantIds)
                {
                    installedPermissions = await membershipService.GetAllPermissions(tenantId);
                    installedPermissionNames = installedPermissions.Select(x => x.Name).ToHashSet();

                    permissionsToAdd = allPermissions
                       .Where(x => !installedPermissionNames.Contains(x.Name))
                       .Select(x => new FrameworkPermission
                       {
                           TenantId = tenantId,
                           Name = x.Name,
                           Category = x.Category,
                           Description = x.Description
                       })
                       .OrderBy(x => x.Category)
                       .ThenBy(x => x.Name);

                    if (!permissionsToAdd.IsNullOrEmpty())
                    {
                        await membershipService.InsertPermissions(permissionsToAdd);
                    }

                    permissionIdsToDelete = installedPermissions
                       .Where(x => !allPermissionNames.Contains(x.Name))
                       .Select(x => x.Id);

                    if (!permissionIdsToDelete.IsNullOrEmpty())
                    {
                        await membershipService.DeletePermissions(permissionIdsToDelete);
                    }
                }

                #endregion Tenants
            }
        }
    }
}