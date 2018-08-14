namespace Framework
{
    public static class FrameworkConstants
    {
        public static class CacheKeys
        {
            /// <summary>
            /// {0}: Tenant ID, {1}: Culture Code
            /// </summary>
            public const string LocalizableStringsFormat = "Framework_LocalizableStrings_{0}_{1}";

            /// <summary>
            /// {0}: Tenant ID
            /// </summary>
            public const string LocalizableStringsPatternFormat = "Framework_LocalizableStrings_{0}_.*";
        }

        public static class Roles
        {
            public const string Administrators = "Administrators";
        }

        public static class StateProviders
        {
            public const string CurrentCultureCode = "CurrentCultureCode";
        }
    }
}