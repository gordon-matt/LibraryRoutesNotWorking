using Framework.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Configuration.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.Configuration)]
    [Route("admin/configuration/themes")]
    public class ThemeController : FrameworkController
    {
        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(FrameworkWebPermissions.ThemesRead))
            {
                return Unauthorized();
            }

            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.General.Configuration].Value);
            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.General.Themes].Value);

            ViewBag.Title = T[FrameworkWebLocalizableStrings.General.Configuration].Value;
            ViewBag.SubTitle = T[FrameworkWebLocalizableStrings.General.Themes].Value;

            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Set = T[FrameworkWebLocalizableStrings.General.Set].Value,
                SetDesktopThemeError = T[FrameworkWebLocalizableStrings.Themes.SetDesktopThemeError].Value,
                SetDesktopThemeSuccess = T[FrameworkWebLocalizableStrings.Themes.SetDesktopThemeSuccess].Value,
                SetMobileThemeError = T[FrameworkWebLocalizableStrings.Themes.SetMobileThemeError].Value,
                SetMobileThemeSuccess = T[FrameworkWebLocalizableStrings.Themes.SetMobileThemeSuccess].Value,
                Columns = new
                {
                    IsDefaultTheme = T[FrameworkWebLocalizableStrings.Themes.Model.IsDefaultTheme].Value,
                    PreviewImageUrl = T[FrameworkWebLocalizableStrings.Themes.Model.PreviewImageUrl].Value,
                    SupportRtl = T[FrameworkWebLocalizableStrings.Themes.Model.SupportRtl].Value
                }
            });
        }
    }
}