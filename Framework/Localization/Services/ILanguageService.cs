﻿using System.Collections.Generic;
using System.Linq;
using Extenso.Data.Entity;
using Framework.Data.Services;
using LanguageEntity = Framework.Localization.Domain.Language;

namespace Framework.Localization.Services
{
    public interface ILanguageService : IGenericDataService<LanguageEntity>
    {
        IEnumerable<LanguageEntity> GetActiveLanguages(int tenantId);

        bool CheckIfRightToLeft(int tenantId, string cultureCode);
    }

    public class LanguageService : GenericDataService<LanguageEntity>, ILanguageService
    {
        public LanguageService(IRepository<LanguageEntity> repository)
            : base(repository)
        {
        }

        public IEnumerable<LanguageEntity> GetActiveLanguages(int tenantId)
        {
            return Find(x => x.TenantId == tenantId && x.IsEnabled);
        }

        public bool CheckIfRightToLeft(int tenantId, string cultureCode)
        {
            //var rtlLanguages = CacheManager.Get("Repository_Language_RightToLeft_" + tenantId, () =>
            //{
            //    using (var connection = OpenConnection())
            //    {
            //        return connection.Query(x => x.TenantId == tenantId && x.IsRTL)
            //            .Select(k => k.CultureCode)
            //            .ToList();
            //    }
            //});

            //return rtlLanguages.Contains(cultureCode);

            using (var connection = OpenConnection())
            {
                var result = connection.Query(x => x.TenantId == tenantId && x.CultureCode == cultureCode).FirstOrDefault();
                return result != null && result.IsRTL;
            }
        }
    }
}