using Extenso.Data.Entity;
using Extenso.Web.OData;
using Framework.Tenants.Domain;

namespace Framework.Web.Areas.Admin.Tenants.Controllers.Api
{
    public class TenantApiController : GenericODataController<Tenant, int>
    {
        public TenantApiController(IRepository<Tenant> repository)
            : base(repository)
        {
        }

        protected override int GetId(Tenant entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Tenant entity)
        {
        }
    }
}