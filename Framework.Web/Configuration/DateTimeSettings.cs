using System.ComponentModel.DataAnnotations;
using Framework.ComponentModel;

namespace Framework.Web.Configuration
{
    public class DateTimeSettings : ISettings
    {
        [Required]
        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.DateTime.DefaultTimeZoneId)]
        public string DefaultTimeZoneId { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.DateTime.AllowUsersToSetTimeZone)]
        public bool AllowUsersToSetTimeZone { get; set; }

        #region ISettings Members

        public string Name => "Date/Time Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Framework.Web.Views.Shared.EditorTemplates.DateTimeSettings";

        #endregion ISettings Members
    }
}