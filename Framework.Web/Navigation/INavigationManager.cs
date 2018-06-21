using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace Framework.Web.Navigation
{
    public interface INavigationManager
    {
        IEnumerable<MenuItem> BuildMenu(string menuName);

        string GetUrl(string menuItemUrl, RouteValueDictionary routeValueDictionary);
    }
}