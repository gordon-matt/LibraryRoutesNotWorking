using System;
using System.Reflection;
using Autofac;
using Extenso.Collections;
using Framework.Infrastructure;

namespace Framework.Data.Entity.EntityFramework
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var entityTypeConfigurations = typeFinder
                .FindClassesOfType(typeof(IFrameworkEntityTypeConfiguration))
                .ToHashSet();

            foreach (var configuration in entityTypeConfigurations)
            {
                if (configuration.GetTypeInfo().IsGenericType)
                {
                    continue;
                }

                var isEnabled = (Activator.CreateInstance(configuration) as IFrameworkEntityTypeConfiguration).IsEnabled;

                if (isEnabled)
                {
                    builder.RegisterType(configuration).As(typeof(IFrameworkEntityTypeConfiguration)).InstancePerLifetimeScope();
                }
            }
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar Members
    }
}