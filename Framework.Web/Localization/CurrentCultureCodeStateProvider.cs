using System;
using Microsoft.AspNetCore.Http;

namespace Framework.Web.Localization
{
    public class CurrentCultureCodeStateProvider : IWorkContextStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebCultureManager cultureManager;

        public CurrentCultureCodeStateProvider(
            IHttpContextAccessor httpContextAccessor,
            IWebCultureManager cultureManager)
        {
            this.cultureManager = cultureManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        #region IWorkContextStateProvider Members

        public Func<IWorkContext, T> Get<T>(string name)
        {
            if (name == FrameworkConstants.StateProviders.CurrentCultureCode)
            {
                return ctx => (T)(object)cultureManager.GetCurrentCulture(httpContextAccessor.HttpContext);
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}