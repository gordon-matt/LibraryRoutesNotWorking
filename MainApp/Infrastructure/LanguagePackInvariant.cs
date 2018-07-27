using System.Collections.Generic;
using Framework.Localization;

namespace MainApp.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode => null;

        public IDictionary<string, string> LocalizedStrings => new Dictionary<string, string>
        {
            { LocalizableStrings.Manage, "Manage" },

            { LocalizableStrings.Dashboard.Administration, "Administration" },
            { LocalizableStrings.Dashboard.Frontend, "Frontend" },
            { LocalizableStrings.Dashboard.Title, "Dashboard" },
            { LocalizableStrings.Dashboard.ToggleNavigation, "Toggle Navigation" },
        };

        #endregion ILanguagePack Members
    }
}