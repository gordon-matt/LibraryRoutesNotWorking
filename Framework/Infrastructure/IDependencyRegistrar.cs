using Autofac;
using Framework.Tenants.Services;

namespace Framework.Infrastructure
{
    public interface IDependencyRegistrar<TContainerBuilder>
    {
        void Register(TContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }

    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // Tenants
            builder.RegisterType<TenantService>().As<ITenantService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}