using Extenso.Data.Entity;

namespace Framework.Tenants.Domain
{
    public interface ITenantEntity : IEntity
    {
        int? TenantId { get; set; }
    }
}