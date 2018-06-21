using Extenso.Data.Entity;
using Framework.Caching;
using Framework.Data.Services;
using Framework.Localization.Domain;

namespace Framework.Localization.Services
{
    public interface ILocalizableStringService : IGenericDataService<LocalizableString>
    {
    }

    public class LocalizableStringService : GenericDataService<LocalizableString>, ILocalizableStringService
    {
        public LocalizableStringService(
            ICacheManager cacheManager,
            IRepository<LocalizableString> repository)
            : base(cacheManager, repository)
        {
        }
    }
}