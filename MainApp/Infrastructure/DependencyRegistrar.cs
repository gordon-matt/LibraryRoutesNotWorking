using Autofac;
using Extenso.Data.Entity;
using Framework.Infrastructure;
using Framework.Web.Infrastructure;
using MainApp.Areas.Admin;
using MainApp.Data;

namespace MainApp.Infrastructure
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
        }
    }
}