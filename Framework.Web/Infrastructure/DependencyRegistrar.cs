using Autofac;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.Caching;
using Framework.Infrastructure;
using Framework.Localization;
using Framework.Web.Areas.Admin;
using Framework.Web.Configuration;
using Framework.Web.Configuration.Services;
using Framework.Web.Localization;
using Framework.Web.Localization.Services;
using Framework.Web.Mvc.EmbeddedResources;
using Framework.Web.Mvc.Themes;
using Framework.Web.Navigation;
using Framework.Web.Security.Membership;
using Framework.Web.Security.Membership.Permissions;

namespace Framework.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // Helpers
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            // Caching
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("Framework_Cache_Static").SingleInstance();

            // Work Context, Themes, Routing, etc
            builder.RegisterType<WebWorkContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();
            builder.RegisterType<EmbeddedResourceResolver>().As<IEmbeddedResourceResolver>().SingleInstance();

            // Security
            builder.RegisterType<RolesBasedAuthorizationService>().As<IAuthorizationService>().SingleInstance();

            // Configuration
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterType<DefaultSettingService>().As<ISettingService>();
            builder.RegisterType<SiteSettings>().As<ISettings>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipSettings>().As<ISettings>().InstancePerLifetimeScope();

            // Navigation
            builder.RegisterType<AureliaRouteProvider>().As<IAureliaRouteProvider>().SingleInstance();
            builder.RegisterType<NavigationManager>().As<INavigationManager>().InstancePerDependency();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();

            // Permission Providers
            builder.RegisterType<StandardPermissions>().As<IPermissionProvider>().SingleInstance();

            // Work Context State Providers
            builder.RegisterType<CurrentUserStateProvider>().As<IWorkContextStateProvider>();
            builder.RegisterType<CurrentThemeStateProvider>().As<IWorkContextStateProvider>();
            builder.RegisterType<CurrentCultureCodeStateProvider>().As<IWorkContextStateProvider>();

            // Localization
            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().InstancePerDependency();
            builder.RegisterType<WebCultureManager>().AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterType<SiteCultureSelector>().As<ICultureSelector>().SingleInstance();
            builder.RegisterType<CookieCultureSelector>().As<ICultureSelector>().SingleInstance();
            
            // Rendering
            builder.RegisterType<RazorViewRenderService>().As<IRazorViewRenderService>().SingleInstance();

            builder.RegisterType<ODataRegistrar>().As<IODataRegistrar>().SingleInstance();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}