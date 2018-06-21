using System;
using Framework.Configuration.Domain;
using Framework.Localization.Domain;
using Framework.Logging.Domain;
using Framework.Security.Membership;
using Framework.Tasks.Domain;
using Framework.Tenants.Domain;
using Framework.Web.Areas.Admin.Configuration.Models;
using Framework.Web.Areas.Admin.Localization.Models;
using Framework.Web.Areas.Admin.Membership.Controllers.Api;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc;
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

            // Configuration
            builder.EntitySet<Setting>("SettingsApi");
            builder.EntitySet<EdmThemeConfiguration>("ThemeApi");

            // Localization
            builder.EntitySet<Language>("LanguageApi");
            builder.EntitySet<LocalizableString>("LocalizableStringApi");

            // Log
            //builder.EntitySet<LogEntry>("LogApi");// TODO

            // Membership
            builder.EntitySet<FrameworkPermission>("PermissionApi");
            builder.EntitySet<FrameworkRole>("RoleApi");
            builder.EntitySet<FrameworkUser>("UserApi");
            builder.EntitySet<PublicUserInfo>("PublicUserApi");

            // Scheduled Tasks
            builder.EntitySet<ScheduledTask>("ScheduledTaskApi");

            // Tenants
            builder.EntitySet<Tenant>("TenantApi");

            RegisterLanguageODataActions(builder);
            RegisterLocalizableStringODataActions(builder);
            RegisterLogODataActions(builder);
            RegisterMembershipODataActions(builder);
            //RegisterPluginODataActions(builder);
            RegisterScheduledTaskODataActions(builder);
            RegisterThemeODataActions(builder);

            routes.MapODataServiceRoute("OData_Framework_Web", "odata/framework/web", builder.GetEdmModel());
        }

        #endregion IODataRegistrar Members

        private static void RegisterLogODataActions(ODataModelBuilder builder)
        {
            var clearAction = builder.EntityType<LogEntry>().Collection.Action("Clear");
            clearAction.Returns<IActionResult>();
        }

        private static void RegisterMembershipODataActions(ODataModelBuilder builder)
        {
            var getUsersInRoleFunction = builder.EntityType<FrameworkUser>().Collection.Function("GetUsersInRole");
            getUsersInRoleFunction.Parameter<string>("roleId");
            getUsersInRoleFunction.Returns<IActionResult>();

            var assignUserToRolesAction = builder.EntityType<FrameworkUser>().Collection.Action("AssignUserToRoles");
            assignUserToRolesAction.Parameter<string>("userId");
            assignUserToRolesAction.CollectionParameter<string>("roles");
            assignUserToRolesAction.Returns<IActionResult>();

            var changePasswordAction = builder.EntityType<FrameworkUser>().Collection.Action("ChangePassword");
            changePasswordAction.Parameter<string>("userId");
            changePasswordAction.Parameter<string>("password");
            changePasswordAction.Returns<IActionResult>();

            var getRolesForUserFunction = builder.EntityType<FrameworkRole>().Collection.Function("GetRolesForUser");
            getRolesForUserFunction.Parameter<string>("userId");
            getRolesForUserFunction.Returns<IActionResult>();

            var assignPermissionsToRoleAction = builder.EntityType<FrameworkRole>().Collection.Action("AssignPermissionsToRole");
            assignPermissionsToRoleAction.Parameter<string>("roleId");
            assignPermissionsToRoleAction.CollectionParameter<string>("permissions");
            assignPermissionsToRoleAction.Returns<IActionResult>();

            var getPermissionsForRoleFunction = builder.EntityType<FrameworkPermission>().Collection.Function("GetPermissionsForRole");
            getPermissionsForRoleFunction.Parameter<string>("roleId");
            getPermissionsForRoleFunction.Returns<IActionResult>();
        }

        private static void RegisterScheduledTaskODataActions(ODataModelBuilder builder)
        {
            var runNowAction = builder.EntityType<ScheduledTask>().Collection.Action("RunNow");
            runNowAction.Parameter<int>("taskId");
            runNowAction.Returns<IActionResult>();
        }

        private static void RegisterThemeODataActions(ODataModelBuilder builder)
        {
            var setDesktopThemeAction = builder.EntityType<EdmThemeConfiguration>().Collection.Action("SetDesktopTheme");
            setDesktopThemeAction.Parameter<string>("themeName");
            setDesktopThemeAction.Returns<IActionResult>();

            var setMobileThemeAction = builder.EntityType<EdmThemeConfiguration>().Collection.Action("SetMobileTheme");
            setMobileThemeAction.Parameter<string>("themeName");
            setMobileThemeAction.Returns<IActionResult>();
        }

        private static void RegisterLanguageODataActions(ODataModelBuilder builder)
        {
            var resetLocalizableStringsAction = builder.EntityType<Language>().Collection.Action("ResetLocalizableStrings");
            resetLocalizableStringsAction.Returns<IActionResult>();
        }

        private static void RegisterLocalizableStringODataActions(ODataModelBuilder builder)
        {
            var getComparitiveTableFunction = builder.EntityType<LocalizableString>().Collection.Function("GetComparitiveTable");
            getComparitiveTableFunction.Parameter<string>("cultureCode");
            getComparitiveTableFunction.Returns<IActionResult>();

            var putComparitiveAction = builder.EntityType<LocalizableString>().Collection.Action("PutComparitive");
            putComparitiveAction.Parameter<string>("cultureCode");
            putComparitiveAction.Parameter<string>("key");
            putComparitiveAction.Parameter<ComparitiveLocalizableString>("entity");

            var deleteComparitiveAction = builder.EntityType<LocalizableString>().Collection.Action("DeleteComparitive");
            deleteComparitiveAction.Parameter<string>("cultureCode");
            deleteComparitiveAction.Parameter<string>("key");
        }
    }
}