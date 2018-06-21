using Framework.Infrastructure;
using Framework.Web;
using Framework.Web.Navigation;
using Microsoft.Extensions.Localization;

namespace FrameworkDemo.Areas.Admin
{
    public class AdminNavigationProvider : INavigationProvider
    {
        public AdminNavigationProvider()
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
            builder.Add(T[FrameworkWebLocalizableStrings.Dashboard.Title], "0", BuildDashboardMenu);
        }

        private static void BuildDashboardMenu(NavigationItemBuilder builder)
        {
            builder.Icons("fa fa-dashboard");
        }

        #endregion INavigationProvider Members
    }
}