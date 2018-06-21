﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extenso.Collections;
using Framework.Tenants;
using Framework.Tenants.Domain;
using Framework.Tenants.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

namespace Framework.Web.Tenants
{
    public class FrameworkTenantResolver : MemoryCacheTenantResolver<Tenant>
    {
        private readonly ITenantService tenantService;
        private IEnumerable<Tenant> tenants;

        public FrameworkTenantResolver(
            IMemoryCache cache,
            ILoggerFactory loggerFactory,
            ITenantService tenantService)
            : base(cache, loggerFactory)
        {
            this.tenantService = tenantService;
            tenants = tenantService.Find();
        }

        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Value.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Tenant> context)
        {
            return context.Tenant.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected override Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            TenantContext<Tenant> tenantContext = null;

            var loggerFactory = Framework.Infrastructure.EngineContext.Current.Resolve<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(this.GetType());

            try
            {
                string host = context.Request.Host.Value.ToLower();

                if (string.IsNullOrEmpty(host))
                {
                    host = "unknown-host";
                }

                logger.LogInformation("[Host]: " + host);

                Tenant tenant;

                if (tenants.IsNullOrEmpty())
                {
                    tenants = tenantService.Find();
                }

                if (tenants.IsNullOrEmpty())
                {
                    logger.LogError("No tenants found! Inserting a new one...");
                    tenant = new Tenant
                    {
                        Name = "Default Tenant",
                        Hosts = host,
                        Url = host
                    };
                    tenantService.Insert(tenant);
                }
                else
                {
                    tenant = tenants.FirstOrDefault(s => s.ContainsHostValue(host));
                }

                if (tenant == null)
                {
                    tenant = tenants.First();
                }

                logger.LogInformation("[Tenant]: ID: {0}, Name: {1}, Hosts: {2}", tenant.Id, tenant.Name, tenant.Hosts);
                tenantContext = new TenantContext<Tenant>(tenant);
            }
            catch (Exception x)
            {
                logger.LogError(new EventId(), x, x.GetBaseException().Message);
            }

            return Task.FromResult(tenantContext);
        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(new TimeSpan(24, 0, 0)); // Cache for 24 hours
        }
    }
}