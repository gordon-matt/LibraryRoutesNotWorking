﻿using System;
using System.Threading.Tasks;
using Extenso.Data.Entity;
using Framework.Caching;
using Framework.Configuration.Domain;
using Framework.Web.Configuration;
using Framework.Web.OData;
using Framework.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Configuration.Controllers.Api
{
    public class SettingsApiController : GenericTenantODataController<Setting, Guid>
    {
        private readonly ICacheManager cacheManager;

        public SettingsApiController(IRepository<Setting> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        protected override Guid GetId(Setting entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Setting entity)
        {
            entity.Id = Guid.NewGuid();
        }

        public override async Task<IActionResult> Put([FromODataUri] Guid key, [FromBody] Setting entity)
        {
            var result = await base.Put(key, entity);

            string cacheKey = string.Format(FrameworkWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            // TODO: This is an ugly hack. We need to have a way for each setting to perform some tasks after update
            if (entity.Name == new SiteSettings().Name)
            {
                cacheManager.Remove(FrameworkWebConstants.CacheKeys.CurrentCulture);
            }

            return result;
        }

        public override async Task<IActionResult> Post([FromBody] Setting entity)
        {
            var result = await base.Post(entity);

            string cacheKey = string.Format(FrameworkWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Setting> patch)
        {
            var result = await base.Patch(key, patch);

            var entity = await Service.FindOneAsync(key);
            string cacheKey = string.Format(FrameworkWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            var result = base.Delete(key);

            var entity = await Service.FindOneAsync(key);
            string cacheKey = string.Format(FrameworkWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            return await result;
        }

        protected override Permission ReadPermission
        {
            get { return FrameworkWebPermissions.SettingsRead; }
        }

        protected override Permission WritePermission
        {
            get { return FrameworkWebPermissions.SettingsWrite; }
        }
    }
}