using System;
using System.Linq;
using Extenso;
using Extenso.Data.Entity;
using Framework.Caching;
using Framework.Configuration.Domain;
using Framework.Infrastructure;

namespace Framework.Web.Configuration.Services
{
    public interface ISettingService
    {
        TSettings GetSettings<TSettings>(int? tenantId = null) where TSettings : ISettings, new();

        ISettings GetSettings(Type settingsType, int? tenantId = null);

        void SaveSettings(string key, string value, int? tenantId = null);

        void SaveSettings<TSettings>(TSettings settings, int? tenantId = null) where TSettings : ISettings;
    }

    public class DefaultSettingService : ISettingService
    {
        private readonly ICacheManager cacheManager;
        private readonly IRepository<Setting> repository;

        public DefaultSettingService(ICacheManager cacheManager, IRepository<Setting> repository)
        {
            this.cacheManager = cacheManager;
            this.repository = repository;
        }

        public TSettings GetSettings<TSettings>(int? tenantId = null) where TSettings : ISettings, new()
        {
            string type = typeof(TSettings).FullName;
            string key = string.Format(FrameworkWebConstants.CacheKeys.SettingsKeyFormat, tenantId, type);
            var result = cacheManager.Get(key, () =>
           {
               Setting settings = null;

               if (tenantId.HasValue)
               {
                   settings = repository.FindOne(x => x.TenantId == tenantId && x.Type == type);
               }
               else
               {
                   settings = repository.FindOne(x => x.TenantId == null && x.Type == type);
               }

               if (settings == null || string.IsNullOrEmpty(settings.Value))
               {
                   return new TSettings();
               }

               return settings.Value.JsonDeserialize<TSettings>();
           });

            // This should not be necessary, but for some reason, the log file shows the following error:
            //  "...A delegate registered to create instances of 'Framework.Web.Configuration.SiteSettings' returned null..."
            return result == null ? new TSettings() : result;
        }

        public ISettings GetSettings(Type settingsType, int? tenantId = null)
        {
            string type = settingsType.FullName;
            string key = string.Format(FrameworkWebConstants.CacheKeys.SettingsKeyFormat, tenantId, type);
            var result = cacheManager.Get(key, () =>
            {
                Setting settings = null;

                if (tenantId.HasValue)
                {
                    settings = repository.FindOne(x => x.TenantId == tenantId && x.Type == type);
                }
                else
                {
                    settings = repository.FindOne(x => x.TenantId == null && x.Type == type);
                }

                if (settings == null || string.IsNullOrEmpty(settings.Value))
                {
                    return (ISettings)Activator.CreateInstance(settingsType);
                }

                return (ISettings)settings.Value.JsonDeserialize(settingsType);
            });

            // This should not be necessary, but for some reason, the log file shows the following error:
            //  "...A delegate registered to create instances of 'Framework.Web.Configuration.SiteSettings' returned null..."
            return result == null ? (ISettings)Activator.CreateInstance(settingsType) : result;
        }

        public void SaveSettings(string key, string value, int? tenantId = null)
        {
            Setting setting = null;

            if (tenantId.HasValue)
            {
                setting = repository.FindOne(x => x.TenantId == tenantId && x.Type == key);
            }
            else
            {
                setting = repository.FindOne(x => x.TenantId == null && x.Type == key);
            }

            if (setting == null)
            {
                var iSettings = EngineContext.Current.ResolveAll<ISettings>().FirstOrDefault(x => x.GetType().FullName == key);

                if (iSettings != null)
                {
                    setting = new Setting { TenantId = tenantId, Name = iSettings.Name, Type = key, Value = value };
                    repository.Insert(setting);
                    cacheManager.RemoveByPattern(string.Format(FrameworkWebConstants.CacheKeys.SettingsKeysPatternFormat, tenantId));
                }
            }
            else
            {
                setting.Value = value;
                repository.Update(setting);
                cacheManager.RemoveByPattern(string.Format(FrameworkWebConstants.CacheKeys.SettingsKeysPatternFormat, tenantId));
            }
        }

        public void SaveSettings<TSettings>(TSettings settings, int? tenantId = null) where TSettings : ISettings
        {
            var type = settings.GetType();
            var key = type.FullName;
            var value = settings.JsonSerialize();
            SaveSettings(key, value, tenantId);
        }
    }
}