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
            builder.Add(T[FrameworkWebLocalizableStrings.Membership.Title], "1", BuildMembershipMenu);
            builder.Add(T[FrameworkWebLocalizableStrings.General.Configuration], "3", BuildConfigurationMenu);
            builder.Add(T[FrameworkWebLocalizableStrings.Maintenance.Title], "4", BuildMaintenanceMenu);
            //builder.Add(T[FrameworkWebLocalizableStrings.Plugins.Title], "99999", BuildPluginsMenu);
        }

        #endregion INavigationProvider Members

        private static void BuildHomeMenu(NavigationItemBuilder builder)
        {
            builder.Permission(StandardPermissions.DashboardAccess);

            builder.Icons("fa fa-dashboard")
                .Url("#");
        }

        private void BuildMembershipMenu(NavigationItemBuilder builder)
        {
            builder
                .Url("#membership")
                .Icons("fa fa-users")
                .Permission(FrameworkWebPermissions.MembershipManage);
        }

        private void BuildConfigurationMenu(NavigationItemBuilder builder)
        {
            builder.Icons("fa fa-cog");

            // Localization
            builder.Add(T[FrameworkWebLocalizableStrings.Localization.Title], "5", item => item
                .Url("#localization/languages")
                .Icons("fa fa-globe")
                .Permission(FrameworkWebPermissions.LanguagesRead));

            // Settings
            builder.Add(T[FrameworkWebLocalizableStrings.General.Settings], "5", item => item
                .Url("#configuration/settings")
                .Icons("fa fa-cog")
                .Permission(FrameworkWebPermissions.SettingsRead));

            // Tenants
            builder.Add(T[FrameworkWebLocalizableStrings.Tenants.Title], "5", item => item
                .Url("#tenants")
                .Icons("fa fa-building-o")
                .Permission(StandardPermissions.FullAccess));

            // Themes
            builder.Add(T[FrameworkWebLocalizableStrings.General.Themes], "5", item => item
                .Url("#configuration/themes")
                .Icons("fa fa-tint")
                .Permission(FrameworkWebPermissions.ThemesRead));
        }

        private void BuildMaintenanceMenu(NavigationItemBuilder builder)
        {
            builder.Icons("fa fa-wrench");

            // TODO
            //builder.Add(T[FrameworkWebLocalizableStrings.Log.Title], "5", item => item
            //    .Url("#log")
            //    .Icons("fa fa-warning")
            //    .Permission(FrameworkWebPermissions.LogRead));

            // Scheduled Tasks
            builder.Add(T[FrameworkWebLocalizableStrings.ScheduledTasks.Title], "5", item => item
                .Url("#scheduled-tasks")
                .Icons("fa fa-clock-o")
                .Permission(FrameworkWebPermissions.ScheduledTasksRead));
        }
    }
}