using Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Framework.Web.Mvc
{
    public class FrameworkController : Controller
    {
        public ILogger Logger { get; private set; }
        
        public IWebWorkContext WorkContext { get; private set; }

        protected FrameworkController()
        {
            WorkContext = EngineContext.Current.Resolve<IWebWorkContext>();
            var loggerFactory = EngineContext.Current.Resolve<ILoggerFactory>();
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected virtual IActionResult RedirectToHomePage()
        {
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}