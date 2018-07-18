﻿using Framework.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Tenants.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.Tenants)]
    [Route("admin/tenants")]
    public class TenantController : FrameworkController
    {
        [Route("")]
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}