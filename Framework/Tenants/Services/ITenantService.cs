using Extenso.Data.Entity;
using Framework.Data.Services;
using Framework.Tenants.Domain;

namespace Framework.Tenants.Services
{
    public interface ITenantService : IGenericDataService<Tenant>
    {
    }

    public class TenantService : GenericDataService<Tenant>, ITenantService
    {
        public TenantService(IRepository<Tenant> repository)
            : base(repository)
        {
        }
    }
}