using System;
using Framework.Infrastructure;
using Framework.Tasks;
using Framework.Tasks.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureFrameworkServices(
            this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.Configure<FrameworkTasksOptions>(options => configuration.GetSection("FrameworkTasksOptions").Bind(options));

            //// Tell framework it is a website (not something like unit test or whatever)
            //Hosting.HostingEnvironment.IsHosted = true;

            EngineContext.Default = new AutofacEngine(services);
            EngineContext.Initialize();

            // Create the IServiceProvider based on the container.
            var serviceProvider = EngineContext.Current.ServiceProvider;

            //if (DataSettingsHelper.IsDatabaseInstalled && FrameworkConfigurationSection.Instance.ScheduledTasks.Enabled)
            //{
            TaskManager.Instance.Initialize();
            TaskManager.Instance.Start();
            //}

            return serviceProvider;
        }
    }
}