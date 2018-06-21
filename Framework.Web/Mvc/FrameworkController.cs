﻿using Framework.Infrastructure;
using Framework.Web.Security.Membership.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Framework.Web.Mvc
{
    public class FrameworkController : Controller
    {
        public ILogger Logger { get; private set; }

        public IStringLocalizer T { get; private set; }

        public IWebWorkContext WorkContext { get; private set; }

        protected FrameworkController()
        {
            WorkContext = EngineContext.Current.Resolve<IWebWorkContext>();
            T = EngineContext.Current.Resolve<IStringLocalizer>();
            var loggerFactory = EngineContext.Current.Resolve<ILoggerFactory>();
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected virtual bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            return authorizationService.TryCheckAccess(permission, WorkContext.CurrentUser);
        }

        protected virtual IActionResult RedirectToHomePage()
        {
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}