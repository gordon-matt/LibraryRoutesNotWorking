using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Options
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureFrameworkOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FrameworkOptions>(configuration.GetSection("Framework"));
        }
    }
}