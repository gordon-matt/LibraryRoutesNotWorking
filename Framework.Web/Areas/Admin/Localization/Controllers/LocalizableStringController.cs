using System;
using System.Linq;
using System.Text;
using Extenso;
using Framework.Localization.Services;
using Framework.Web.Areas.Admin.Localization.Models;
using Framework.Web.Configuration;
using Framework.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Localization.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.Localization)]
    [Route("admin/localization/localizable-strings")]
    public class LocalizableStringController : FrameworkController
    {
        private readonly Lazy<ILanguageService> languageService;
        private readonly Lazy<ILocalizableStringService> localizableStringService;
        private readonly SiteSettings siteSettings;

        public LocalizableStringController(
            Lazy<ILanguageService> languageService,
            Lazy<ILocalizableStringService> localizableStringService,
            SiteSettings siteSettings)
        {
            this.languageService = languageService;
            this.localizableStringService = localizableStringService;
            this.siteSettings = siteSettings;
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(FrameworkWebPermissions.LocalizableStringsRead))
            {
                return Unauthorized();
            }

            //var language = languageService.Value.FindOne(languageId);

            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.Localization.Languages].Value, "#localization/languages");
            //WorkContext.Breadcrumbs.Add(language.Name);
            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.Localization.LocalizableStrings].Value);

            ViewBag.Title = T[FrameworkWebLocalizableStrings.Localization.Title].Value;
            ViewBag.SubTitle = T[FrameworkWebLocalizableStrings.Localization.LocalizableStrings].Value;

            //ViewBag.CultureCode = language.CultureCode;

            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Columns = new
                {
                    InvariantValue = T[FrameworkWebLocalizableStrings.Localization.LocalizableStringModel.InvariantValue].Value,
                    Key = T[FrameworkWebLocalizableStrings.Localization.LocalizableStringModel.Key].Value,
                    LocalizedValue = T[FrameworkWebLocalizableStrings.Localization.LocalizableStringModel.LocalizedValue].Value,
                }
            });
        }

        [Route("export/{cultureCode}")]
        public ActionResult ExportLanguagePack(string cultureCode)
        {
            int tenantId = WorkContext.CurrentTenant.Id;

            var localizedStrings = localizableStringService.Value.Find(x =>
                x.TenantId == tenantId &&
                x.CultureCode == cultureCode &&
                x.TextValue != null);

            var languagePack = new LanguagePackFile
            {
                CultureCode = cultureCode,
                LocalizedStrings = localizedStrings.ToDictionary(k => k.TextKey, v => v.TextValue)
            };

            string json = languagePack.ToJson();
            string fileName = string.Format("{0}_LanguagePack_{1}_{2:yyyy-MM-dd}.json", siteSettings.SiteName, cultureCode, DateTime.Now);
            return File(new UTF8Encoding().GetBytes(json), "application/json", fileName);
        }
    }
}