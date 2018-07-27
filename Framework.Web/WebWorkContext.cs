using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Extenso;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Tenants;
using Framework.Tenants.Domain;
using Framework.Tenants.Services;

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

        public string CurrentTheme
        {
            get => GetState<string>(FrameworkWebConstants.StateProviders.CurrentTheme);
            set => SetState(FrameworkWebConstants.StateProviders.CurrentTheme, value);
        }

        public string CurrentCultureCode => GetState<string>(FrameworkConstants.StateProviders.CurrentCultureCode);

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

                    var tenantService = EngineContext.Current.Resolve<ITenantService>();
                    var allTenants = tenantService.Find();
                    var tenant = allTenants.FirstOrDefault(s => s.ContainsHostValue(host));

                    if (tenant == null)
                    {
                        // Load the first found tenant
                        tenant = allTenants.FirstOrDefault();
                    }
                    if (tenant == null)
                    {
                        throw new ApplicationException("No tenant could be loaded");
                    }

                    cachedTenant = tenant;
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

        //private Func<object> FindResolverForState<T>(string name)
        //{
        //    return () =>
        //    {
        //        var resolver = workContextStateProviders
        //             .Select(wcsp => wcsp.Get<T>(name))
        //             .FirstOrDefault(value => !Equals(value, default(T)));

        //        if (resolver == null)
        //        {
        //            return default(T);
        //        }
        //        return resolver(this);
        //    };
        //}
    }
}