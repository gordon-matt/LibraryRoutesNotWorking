using Autofac;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.Infrastructure;
using Framework.Localization;
using Framework.Web.Localization;
using Framework.Web.Localization.Services;
using Framework.Web.Mvc.EmbeddedResources;
using Framework.Web.Security.Membership;

namespace Framework.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // Helpers
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            // Work Context, Themes, Routing, etc
            builder.RegisterType<WebWorkContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EmbeddedResourceResolver>().As<IEmbeddedResourceResolver>().SingleInstance();

            // Navigation
            builder.RegisterType<AureliaRouteProvider>().As<IAureliaRouteProvider>().SingleInstance();

            // Work Context State Providers
            builder.RegisterType<CurrentUserStateProvider>().As<IWorkContextStateProvider>();
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