using System;
using Framework.Infrastructure;
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
            //// Tell framework it is a website (not something like unit test or whatever)
            //Hosting.HostingEnvironment.IsHosted = true;

            EngineContext.Default = new AutofacEngine(services);
            EngineContext.Initialize();

            // Create the IServiceProvider based on the container.
            var serviceProvider = EngineContext.Current.ServiceProvider;
            
            return serviceProvider;
        }
    }
}