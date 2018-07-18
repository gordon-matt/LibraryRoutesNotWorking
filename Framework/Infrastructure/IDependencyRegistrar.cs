using Autofac;
using Framework.Caching;
using Framework.Localization.Services;
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

            // Localization
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerDependency();
            builder.RegisterType<LocalizableStringService>().As<ILocalizableStringService>().InstancePerDependency();
            builder.RegisterType<LocalizablePropertyService>().As<ILocalizablePropertyService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}