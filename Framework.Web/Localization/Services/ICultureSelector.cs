using Microsoft.AspNetCore.Http;

namespace Framework.Web.Localization.Services
{
    public interface ICultureSelector
    {
        CultureSelectorResult GetCulture(HttpContext context);
    }
}