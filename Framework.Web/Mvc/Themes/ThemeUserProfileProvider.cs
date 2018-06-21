using System.Collections.Generic;
using Framework.ComponentModel;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Threading;
using Framework.Web.Security.Membership;

namespace Framework.Web.Mvc.Themes
{
    public class ThemeUserProfileProvider : IUserProfileProvider
    {
        public class Fields
        {
            public const string PreferredTheme = "PreferredTheme";
        }

        [LocalizedDisplayName(FrameworkWebLocalizableStrings.UserProfile.Theme.PreferredTheme)]
        public string PreferredTheme { get; set; }

        #region IUserProfileProvider Members

        public string Name
        {
            get { return "Theme"; }
        }

        public string DisplayTemplatePath
        {
            get { return "Framework.Web.Views.Shared.DisplayTemplates.ThemeUserProfileProvider"; }
        }

        public string EditorTemplatePath
        {
            get { return "Framework.Web.Views.Shared.EditorTemplates.ThemeUserProfileProvider"; }
        }

        public int Order
        {
            get { return 9999; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                Fields.PreferredTheme
            };
        }

        public void PopulateFields(string userId)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            PreferredTheme = AsyncHelper.RunSync(() => membershipService.GetProfileEntry(userId, Fields.PreferredTheme));
        }

        #endregion IUserProfileProvider Members
    }
}