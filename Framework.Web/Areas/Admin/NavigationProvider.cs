using Framework.Infrastructure;
using Framework.Web.Navigation;
using Framework.Web.Security.Membership.Permissions;
using Microsoft.Extensions.Localization;

namespace Framework.Web.Areas.Admin
{
    public class NavigationProvider : INavigationProvider
    {
        public NavigationProvider()
        {
            T = EngineContext.Current.Resolve<IStringLocalizer>();
        }

        public IStringLocalizer T { get; set; }

        #region INavigationProvider Members

        public string MenuName
        {
            get { return FrameworkWebConstants.Areas.Admin; }
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T[FrameworkWebLocalizableStrings.Dashboard.Title], "0", BuildHomeMenu);
            builder.Add(T[FrameworkWebLocalizableStrings.General.Configuration], "3", BuildConfigurationMenu);
            //builder.Add(T[FrameworkWebLocalizableStrings.Plugins.Title], "99999", BuildPluginsMenu);
        }

        #endregion INavigationProvider Members

        private static void BuildHomeMenu(NavigationItemBuilder builder)
        {
            builder.Permission(StandardPermissions.DashboardAccess);

            builder.Icons("fa fa-dashboard")
                .Url("#");
        }
        
        private void BuildConfigurationMenu(NavigationItemBuilder builder)
        {
            builder.Icons("fa fa-cog");

            // Tenants
            builder.Add(T[FrameworkWebLocalizableStrings.Tenants.Title], "5", item => item
                .Url("#tenants")
                .Icons("fa fa-building-o")
                .Permission(StandardPermissions.FullAccess));
        }
    }
}