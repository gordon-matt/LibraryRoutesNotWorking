using Autofac;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.Infrastructure;
using Framework.Web.Mvc.EmbeddedResources;

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