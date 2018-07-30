using System;
using Framework.Infrastructure;
using Framework.Localization.Services;
using Microsoft.Extensions.Localization;

namespace Framework.Localization
{
    public class FrameworkStringLocalizerFactory : IStringLocalizerFactory
    {
        private FrameworkStringLocalizer stringLocalizer;

        public IStringLocalizer Create(Type resourceSource)
        {
            return Create();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return Create();
        }

        protected IStringLocalizer Create()
        {
            if (stringLocalizer == null)
            {
                var localizableStringService = EngineContext.Current.Resolve<ILocalizableStringService>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                stringLocalizer = new FrameworkStringLocalizer(workContext, localizableStringService);
            }
            return stringLocalizer;
        }
    }
}