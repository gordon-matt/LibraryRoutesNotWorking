using Autofac;
using Framework.Caching;
using Framework.Localization.Services;
using Framework.Logging.Services;
using Framework.Tasks;
using Framework.Tasks.Services;
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
            builder.RegisterType<ClearCacheTask>().As<ITask>().SingleInstance();

            // Tenants
            builder.RegisterType<TenantService>().As<ITenantService>().InstancePerDependency();

            // Localization
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerDependency();
            builder.RegisterType<LocalizableStringService>().As<ILocalizableStringService>().InstancePerDependency();
            builder.RegisterType<LocalizablePropertyService>().As<ILocalizablePropertyService>().InstancePerDependency();

            builder.RegisterType<ScheduledTaskService>().As<IScheduledTaskService>().InstancePerDependency();

            builder.RegisterType<LogService>().As<ILogService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}