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
                var T = EngineContext.Current.Resolve<IStringLocalizer>();
                var routes = new List<AureliaRoute>();

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Membership.Scripts.index",
                    Route = "membership",
                    Name = "framework-web/membership",
                    Title = T[FrameworkWebLocalizableStrings.Membership.Title]
                });

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Localization.Scripts.index",
                    Route = "localization/languages",
                    Name = "framework-web/localization/languages",
                    Title = T[FrameworkWebLocalizableStrings.Localization.Languages]
                });

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Localization.Scripts.localizableStrings",
                    Route = "localization/localizable-strings/:cultureCode",
                    Name = "framework-web/localization/localizable-strings",
                    Title = T[FrameworkWebLocalizableStrings.Localization.LocalizableStrings]
                });

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.ScheduledTasks.Scripts.index",
                    Route = "scheduled-tasks",
                    Name = "framework-web/scheduled-tasks",
                    Title = T[FrameworkWebLocalizableStrings.ScheduledTasks.Title]
                });

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Tenants.Scripts.index",
                    Route = "tenants",
                    Name = "framework-web/tenants",
                    Title = T[FrameworkWebLocalizableStrings.Tenants.Title]
                });

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Configuration.Scripts.settings",
                    Route = "configuration/settings",
                    Name = "framework-web/configuration/settings",
                    Title = T[FrameworkWebLocalizableStrings.General.Settings]
                });

                routes.Add(new AureliaRoute
                {
                    ModuleId = "/aurelia-app/embedded/Framework.Web.Areas.Admin.Configuration.Scripts.themes",
                    Route = "configuration/themes",
                    Name = "framework-web/configuration/themes",
                    Title = T[FrameworkWebLocalizableStrings.General.Themes]
                });

                return routes;
            }
        }

        public IDictionary<string, string> ModuleIdToViewUrlMappings => new Dictionary<string, string>
        {
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Membership.Scripts.index", "admin/membership" },
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Localization.Scripts.index", "admin/localization/languages" },
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Localization.Scripts.localizableStrings", "admin/localization/localizable-strings" },
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.ScheduledTasks.Scripts.index", "admin/scheduled-tasks" },
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Tenants.Scripts.index", "admin/tenants" },
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Configuration.Scripts.settings", "admin/configuration/settings" },
            { "aurelia-app/embedded/Framework.Web.Areas.Admin.Configuration.Scripts.themes", "admin/configuration/themes" },
        };

        #endregion IAureliaRouteProvider Members
    }
}