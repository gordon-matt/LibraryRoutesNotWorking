using Framework.ComponentModel;
using Framework.Web.Configuration;

namespace Framework.Web.Security.Membership
{
    public class MembershipSettings : ISettings
    {
        public MembershipSettings()
        {
            GeneratedPasswordLength = 7;
            GeneratedPasswordNumberOfNonAlphanumericChars = 3;
        }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Membership.GeneratedPasswordLength)]
        public byte GeneratedPasswordLength { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Membership.GeneratedPasswordNumberOfNonAlphanumericChars)]
        public byte GeneratedPasswordNumberOfNonAlphanumericChars { get; set; }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.Settings.Membership.DisallowUnconfirmedUserLogin)]
        public bool DisallowUnconfirmedUserLogin { get; set; }

        #region ISettings Members

        public string Name => "Membership Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Framework.Web.Views.Shared.EditorTemplates.MembershipSettings";

        #endregion ISettings Members
    }
}