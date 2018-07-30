using Extenso.Data.Entity;
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
            IRepository<LocalizableString> repository)
            : base(repository)
        {
        }
    }
}