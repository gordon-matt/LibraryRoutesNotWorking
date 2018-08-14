using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Tenants.Controllers
{
    [Area(FrameworkWebConstants.Areas.Tenants)]
    [Route("admin/tenants")]
    public class TenantController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}