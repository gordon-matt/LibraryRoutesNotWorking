using System;
using System.Linq;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Threading;

namespace Framework.Web.Mvc.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly IWorkContext workContext;
        private readonly IThemeProvider themeProvider;

        private bool isDesktopThemeCached;
        private string cachedDesktopThemeName;
        
        public ThemeContext(
            IWorkContext workContext,
            IThemeProvider themeProvider)
        {
            this.workContext = workContext;
            this.themeProvider = themeProvider;
        }

        /// <summary>
        /// Get or set current theme for desktops
        /// </summary>
        public string WorkingTheme
        {
            get
            {
                if (isDesktopThemeCached)
                {
                    return cachedDesktopThemeName;
                }

                string theme = string.Empty;
                
                // Default tenant theme
                if (string.IsNullOrEmpty(theme))
                {
                    theme = "Default";
                }

                // Ensure that theme exists
                if (!themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = themeProvider.GetThemeConfigurations()
                        .FirstOrDefault();

                    if (themeInstance == null)
                    {
                        throw new Exception("No theme could be loaded");
                    }

                    theme = themeInstance.ThemeName;
                }

                // Cache theme
                this.cachedDesktopThemeName = theme;
                this.isDesktopThemeCached = true;
                return theme;
            }
            set
            {
                if (workContext.CurrentUser == null)
                {
                    return;
                }

                var membershipService = EngineContext.Current.Resolve<IMembershipService>();

                //clear cache
                this.isDesktopThemeCached = false;
            }
        }
    }
}