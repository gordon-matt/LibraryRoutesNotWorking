using Framework.ComponentModel;

namespace Framework.Web.Configuration
{
    public class SiteSettings : ISettings
    {
        public SiteSettings()
        {
            SiteName = "My Site";
            DefaultTheme = "Default";
            DefaultGridPageSize = 10;
            DefaultFrontendLayoutPath = "~/Views/Shared/_Layout.cshtml";
            AdminLayoutPath = "~/Areas/Admin/Views/Shared/_Layout.cshtml";
            HomePageTitle = "Home Page";
        }

        #region ISettings Members

        public string Name => "Site Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Framework.Web.Views.Shared.EditorTemplates.SiteSettings";

        #endregion ISettings Members

        #region General

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.SiteName)]
        public string SiteName { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.DefaultFrontendLayoutPath)]
        public string DefaultFrontendLayoutPath { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.AdminLayoutPath)]
        public string AdminLayoutPath { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.DefaultGridPageSize)]
        public int DefaultGridPageSize { get; set; }

        #endregion General

        #region Themes

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.DefaultTheme)]
        public string DefaultTheme { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.AllowUserToSelectTheme)]
        public bool AllowUserToSelectTheme { get; set; }

        #endregion Themes

        #region Localization

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.DefaultLanguage)]
        public string DefaultLanguage { get; set; }

        #endregion Localization

        #region SEO

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.DefaultMetaKeywords)]
        public string DefaultMetaKeywords { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.DefaultMetaDescription)]
        public string DefaultMetaDescription { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Site.HomePageTitle)]
        public string HomePageTitle { get; set; }

        #endregion SEO
    }
}