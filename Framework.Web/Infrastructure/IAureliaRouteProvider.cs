using System.Collections.Generic;
using Framework.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Framework.Web.Infrastructure
{
    // TODO: Consider exchanging this for an AureliaRouteAttribute instead. Need to test performance though,
    //  as it would mean using reflection...
    public interface IAureliaRouteProvider
    {
        string Area { get; }

        IEnumerable<AureliaRoute> Routes { get; }

        IDictionary<string, string> ModuleIdToViewUrlMappings { get; }
    }

    public class AureliaRouteProvider : IAureliaRouteProvider
    {
        #region IAureliaRouteProvider Members

        public string Area
        {
            get { return FrameworkWebConstants.Areas.Admin; }
        }

        public IEnumerable<AureliaRoute> Routes
        {
            get
            {
                var routes = new List<AureliaRoute>();
                
                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Tenants.Scripts.index",
                    Route = "tenants",
                    Name = "framework-web/tenants",
                    Title = "Tenants"
                });
                
                return routes;
            }
        }

        public IDictionary<string, string> ModuleIdToViewUrlMappings => new Dictionary<string, string>
        {
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Tenants.Scripts.index", "admin/tenants" }
        };

        #endregion IAureliaRouteProvider Members
    }
}