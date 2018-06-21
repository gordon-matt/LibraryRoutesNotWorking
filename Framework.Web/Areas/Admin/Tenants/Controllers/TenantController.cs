using Framework.Web.Mvc;
using Framework.Web.Security.Membership.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Tenants.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.Tenants)]
    [Route("admin/tenants")]
    public class TenantController : FrameworkController
    {
        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public IActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.Tenants.Title].Value);

            ViewBag.Title = T[FrameworkWebLocalizableStrings.Tenants.Title].Value;
            ViewBag.SubTitle = T[FrameworkWebLocalizableStrings.Tenants.ManageTenants].Value;

            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Create = T[FrameworkWebLocalizableStrings.General.Create].Value,
                Delete = T[FrameworkWebLocalizableStrings.General.Delete].Value,
                DeleteRecordConfirm = T[FrameworkWebLocalizableStrings.General.ConfirmDeleteRecord].Value,
                DeleteRecordError = T[FrameworkWebLocalizableStrings.General.DeleteRecordError].Value,
                DeleteRecordSuccess = T[FrameworkWebLocalizableStrings.General.DeleteRecordSuccess].Value,
                Edit = T[FrameworkWebLocalizableStrings.General.Edit].Value,
                GetRecordError = T[FrameworkWebLocalizableStrings.General.GetRecordError].Value,
                InsertRecordError = T[FrameworkWebLocalizableStrings.General.InsertRecordError].Value,
                InsertRecordSuccess = T[FrameworkWebLocalizableStrings.General.InsertRecordSuccess].Value,
                UpdateRecordError = T[FrameworkWebLocalizableStrings.General.UpdateRecordError].Value,
                UpdateRecordSuccess = T[FrameworkWebLocalizableStrings.General.UpdateRecordSuccess].Value,
                Columns = new
                {
                    Name = T[FrameworkWebLocalizableStrings.General.Name].Value
                }
            });
        }
    }
}