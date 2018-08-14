namespace Framework.Web
{
    public class FrameworkWebConstants
    {
        public static class Areas
        {
            public const string Admin = "Admin";
            public const string Configuration = "Admin/Configuration";

            //public const string Indexing = "Admin/Indexing";
            public const string Localization = "Admin/Localization";

            public const string Log = "Admin/Log";
            public const string Membership = "Admin/Membership";

            //public const string Plugins = "Admin/Plugins";
            public const string ScheduledTasks = "Admin/ScheduledTasks";

            public const string Tenants = "Admin/Tenants";
        }

        public static class CacheKeys
        {
            public const string CurrentCulture = "Framework.Web.CacheKeys.CurrentCulture";

            /// <summary>
            /// {0} for TenantId, {1} for settings "Type"
            /// </summary>
            public const string SettingsKeyFormat = "Framework.Web.CacheKeys.Settings.Tenant_{0}_{1}";

            public const string SettingsKeysPatternFormat = "Framework.Web.CacheKeys.Settings.Tenant_{0}_.*";
        }

        public static class StateProviders
        {
            public const string CurrentCultureCode = "CurrentCultureCode";
            public const string CurrentTheme = "CurrentTheme";
        }
    }
}