using Framework.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.ScheduledTasks.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.ScheduledTasks)]
    [Route("admin/scheduled-tasks")]
    public class ScheduledTaskController : FrameworkController
    {
        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public IActionResult Index()
        {
            if (!CheckPermission(FrameworkWebPermissions.ScheduledTasksRead))
            {
                return Unauthorized();
            }

            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.ScheduledTasks.Title].Value);

            ViewBag.Title = T[FrameworkWebLocalizableStrings.ScheduledTasks.Title].Value;
            ViewBag.SubTitle = T[FrameworkWebLocalizableStrings.ScheduledTasks.ManageScheduledTasks].Value;

            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Edit = T[FrameworkWebLocalizableStrings.General.Edit].Value,
                ExecutedTaskSuccess = T[FrameworkWebLocalizableStrings.ScheduledTasks.ExecutedTaskSuccess].Value,
                ExecutedTaskError = T[FrameworkWebLocalizableStrings.ScheduledTasks.ExecutedTaskError].Value,
                GetRecordError = T[FrameworkWebLocalizableStrings.General.GetRecordError].Value,
                RunNow = T[FrameworkWebLocalizableStrings.ScheduledTasks.RunNow].Value,
                UpdateRecordError = T[FrameworkWebLocalizableStrings.General.UpdateRecordError].Value,
                UpdateRecordSuccess = T[FrameworkWebLocalizableStrings.General.UpdateRecordSuccess].Value,
                Columns = new
                {
                    Enabled = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.Enabled].Value,
                    LastEndUtc = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.LastEndUtc].Value,
                    LastStartUtc = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.LastStartUtc].Value,
                    LastSuccessUtc = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.LastSuccessUtc].Value,
                    Name = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.Name].Value,
                    Seconds = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.Seconds].Value,
                    StopOnError = T[FrameworkWebLocalizableStrings.ScheduledTasks.Model.StopOnError].Value,
                }
            });
        }
    }
}