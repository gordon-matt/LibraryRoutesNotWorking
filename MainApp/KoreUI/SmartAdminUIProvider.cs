using Framework.Web.Mvc.KoreUI;
using Framework.Web.Mvc.KoreUI.Providers;

namespace FrameworkDemo.KoreUI
{
    public class SmartAdminUIProvider : Bootstrap3UIProvider
    {
        private IPanelProvider panelProvider;

        public override IPanelProvider PanelProvider
        {
            get { return panelProvider ?? (panelProvider = new SmartAdminPanelProvider()); }
        }
    }
}