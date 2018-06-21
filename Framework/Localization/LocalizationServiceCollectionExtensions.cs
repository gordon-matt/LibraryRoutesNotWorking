using System;
using Framework.Localization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddFrameworkLocalization(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(new ServiceDescriptor(typeof(IStringLocalizerFactory), typeof(FrameworkStringLocalizerFactory), ServiceLifetime.Scoped));
            services.TryAdd(new ServiceDescriptor(typeof(IStringLocalizer), typeof(FrameworkStringLocalizer), ServiceLifetime.Scoped));

            return services;
        }
    }
}