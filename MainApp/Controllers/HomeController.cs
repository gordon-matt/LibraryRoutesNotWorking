using Framework.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    //[Authorize]
    public class HomeController : FrameworkController
    {
        [Route("")]
        public IActionResult Index()
        {
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