using Framework.Web.Configuration;
using Framework.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace FrameworkDemo.Controllers
{
    //[Authorize]
    public class HomeController : FrameworkController
    {
        private readonly SiteSettings siteSettings;

        public HomeController(SiteSettings siteSettings)
        {
            this.siteSettings = siteSettings;
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Title = siteSettings.HomePageTitle;
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}