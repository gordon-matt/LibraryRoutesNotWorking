using System;
using Framework.Tenants.Domain;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;

namespace Framework.Web.Infrastructure
{
    public interface IODataRegistrar
    {
        void Register(IRouteBuilder routes, IServiceProvider services);
    }

    public class ODataRegistrar : IODataRegistrar
    {
        #region IODataRegistrar Members

        public void Register(IRouteBuilder routes, IServiceProvider services)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder(services);

            // Tenants
            builder.EntitySet<Tenant>("TenantApi");

            routes.MapODataServiceRoute("OData_Framework_Web", "odata/framework/web", builder.GetEdmModel());
        }

        #endregion IODataRegistrar Members
    }
}