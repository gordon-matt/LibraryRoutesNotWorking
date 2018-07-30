﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Extenso;
using Extenso.Data.Entity;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Tenants;
using Framework.Tenants.Domain;

namespace Framework.Web
{
    public partial class WebWorkContext : IWebWorkContext
    {
        private Tenant cachedTenant;

        private readonly IWebHelper webHelper;
        private readonly ConcurrentDictionary<string, Func<object>> stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        private readonly IEnumerable<IWorkContextStateProvider> workContextStateProviders;

        public WebWorkContext()
        {
            webHelper = EngineContext.Current.Resolve<IWebHelper>();
            workContextStateProviders = EngineContext.Current.ResolveAll<IWorkContextStateProvider>();
        }

        #region IWorkContext Members

        public T GetState<T>(string name)
        {
            var resolver = stateResolvers.GetOrAdd(name, FindResolverForState<T>);
            return (T)resolver();
        }

        public void SetState<T>(string name, T value)
        {
            stateResolvers[name] = () => value;
        }

        public string CurrentTheme => "Default";

        public string CurrentCultureCode => "en-US";

        public FrameworkUser CurrentUser => GetState<FrameworkUser>(FrameworkConstants.StateProviders.CurrentUser);

        public virtual Tenant CurrentTenant
        {
            get
            {
                if (cachedTenant != null)
                {
                    return cachedTenant;
                }

                try
                {
                    // Try to determine the current tenant by HTTP_HOST
                    string host = webHelper.GetUrlHost();

                    if (host.Contains(":"))
                    {
                        host = host.LeftOf(':');
                    }

                    var tenantRepository = EngineContext.Current.Resolve<IRepository<Tenant>>();
                    var allTenants = tenantRepository.Find();
                    var tenant = allTenants.FirstOrDefault(s => s.ContainsHostValue(host));

                    if (tenant == null)
                    {
                        // Load the first found tenant
                        tenant = allTenants.FirstOrDefault();
                    }

                    cachedTenant = tenant ?? throw new ApplicationException("No tenant could be loaded");
                    return cachedTenant;
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion IWorkContext Members

        private Func<object> FindResolverForState<T>(string name)
        {
            var resolver = workContextStateProviders
                .Select(wcsp => wcsp.Get<T>(name))
                .FirstOrDefault(value => !Equals(value, default(T)));

            if (resolver == null)
            {
                return () => default(T);
            }
            return () => resolver(this);
        }
    }
}