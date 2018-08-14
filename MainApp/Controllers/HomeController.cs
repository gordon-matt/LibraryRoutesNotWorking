using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}