using Autofac;
using Extenso.Data.Entity;
using FrameworkDemo.Areas.Admin;
using FrameworkDemo.Data;
using FrameworkDemo.Services;
using Framework.Data.Entity.EntityFramework;
using Framework.Infrastructure;
using Framework.Localization;
using Framework.Security.Membership;
using Framework.Web.Infrastructure;
using Framework.Web.Navigation;

namespace FrameworkDemo.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        public int Order
        {
            get { return 1; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ApplicationDbContextFactory>().As<IDbContextFactory>().SingleInstance();

            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            // SPA Routes
            builder.RegisterType<AdminAureliaRouteProvider>().As<IAureliaRouteProvider>().SingleInstance();

            // Services
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerDependency();

            // Localization
            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().InstancePerDependency();

            // Navigation
            builder.RegisterType<AdminNavigationProvider>().As<INavigationProvider>().SingleInstance();
        }
    }
}