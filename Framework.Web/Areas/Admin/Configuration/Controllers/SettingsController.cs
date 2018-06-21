using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extenso.AspNetCore.Mvc.Rendering;
using Framework.Infrastructure;
using Framework.Web.Configuration;
using Framework.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Configuration.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.Configuration)]
    [Route("admin/configuration/settings")]
    public class SettingsController : FrameworkController
    {
        private readonly Lazy<IEnumerable<ISettings>> settings;

        public SettingsController(Lazy<IEnumerable<ISettings>> settings)
            : base()
        {
            this.settings = settings;
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public IActionResult Index()
        {
            if (!CheckPermission(FrameworkWebPermissions.SettingsRead))
            {
                return Unauthorized();
            }

            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.General.Configuration].Value);
            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.General.Settings].Value);

            ViewBag.Title = T[FrameworkWebLocalizableStrings.General.Configuration].Value;
            ViewBag.SubTitle = T[FrameworkWebLocalizableStrings.General.Settings].Value;

            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Edit = T[FrameworkWebLocalizableStrings.General.Edit].Value,
                GetRecordError = T[FrameworkWebLocalizableStrings.General.GetRecordError].Value,
                UpdateRecordError = T[FrameworkWebLocalizableStrings.General.UpdateRecordError].Value,
                UpdateRecordSuccess = T[FrameworkWebLocalizableStrings.General.UpdateRecordSuccess].Value,
                Columns = new
                {
                    Name = T[FrameworkWebLocalizableStrings.Settings.Model.Name].Value,
                }
            });
        }

        [Route("get-editor-ui/{type}")]
        public async Task<IActionResult> GetEditorUI(string type)
        {
            var model = settings.Value.FirstOrDefault(x => x.GetType().FullName == type.Replace('-', '.'));

            if (model == null)
            {
                return NotFound();
            }

            var razorViewRenderService = EngineContext.Current.Resolve<IRazorViewRenderService>();
            string content = await razorViewRenderService.RenderToStringAsync(model.EditorTemplatePath, model, routeData: RouteData, useActionContext: true);
            return Json(new { Content = content });
        }
    }
}