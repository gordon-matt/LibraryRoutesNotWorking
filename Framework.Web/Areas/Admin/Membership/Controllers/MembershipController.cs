using System.Threading.Tasks;
using Extenso.AspNetCore.Mvc;
using Framework.Security.Membership;
using Framework.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Areas.Admin.Membership.Controllers
{
    [Authorize]
    [Area(FrameworkWebConstants.Areas.Membership)]
    [Route("admin/membership")]
    public class MembershipController : FrameworkController
    {
        private readonly IMembershipService membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        protected virtual bool CheckPermissions()
        {
            return CheckPermission(FrameworkWebPermissions.MembershipManage);
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public virtual async Task<IActionResult> Index()
        {
            if (!CheckPermissions())
            {
                return Unauthorized();
            }

            WorkContext.Breadcrumbs.Add(T[FrameworkWebLocalizableStrings.Membership.Title].Value);

            ViewBag.Title = T[FrameworkWebLocalizableStrings.Membership.Title].Value;

            ViewBag.SelectList = (await membershipService.GetAllRoles(WorkContext.CurrentTenant.Id))
                .ToSelectList(v => v.Id.ToString(), t => t.Name, T[FrameworkWebLocalizableStrings.Membership.AllRolesSelectListOption].Value);

            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                ChangePasswordError = T[FrameworkWebLocalizableStrings.Membership.ChangePasswordError].Value,
                ChangePasswordSuccess = T[FrameworkWebLocalizableStrings.Membership.ChangePasswordSuccess].Value,
                Create = T[FrameworkWebLocalizableStrings.General.Create].Value,
                Delete = T[FrameworkWebLocalizableStrings.General.Delete].Value,
                DeleteRecordConfirm = T[FrameworkWebLocalizableStrings.General.ConfirmDeleteRecord].Value,
                DeleteRecordError = T[FrameworkWebLocalizableStrings.General.DeleteRecordError].Value,
                DeleteRecordSuccess = T[FrameworkWebLocalizableStrings.General.DeleteRecordSuccess].Value,
                Edit = T[FrameworkWebLocalizableStrings.General.Edit].Value,
                GetRecordError = T[FrameworkWebLocalizableStrings.General.GetRecordError].Value,
                InsertRecordError = T[FrameworkWebLocalizableStrings.General.InsertRecordError].Value,
                InsertRecordSuccess = T[FrameworkWebLocalizableStrings.General.InsertRecordSuccess].Value,
                Password = T[FrameworkWebLocalizableStrings.Membership.Password].Value,
                Permissions = T[FrameworkWebLocalizableStrings.Membership.Permissions].Value,
                Roles = T[FrameworkWebLocalizableStrings.Membership.Roles].Value,
                SavePermissionsError = T[FrameworkWebLocalizableStrings.Membership.SavePermissionsError].Value,
                SavePermissionsSuccess = T[FrameworkWebLocalizableStrings.Membership.SavePermissionsSuccess].Value,
                SaveRolesError = T[FrameworkWebLocalizableStrings.Membership.SaveRolesError].Value,
                SaveRolesSuccess = T[FrameworkWebLocalizableStrings.Membership.SaveRolesSuccess].Value,
                UpdateRecordError = T[FrameworkWebLocalizableStrings.General.UpdateRecordError].Value,
                UpdateRecordSuccess = T[FrameworkWebLocalizableStrings.General.UpdateRecordSuccess].Value,
                Columns = new
                {
                    User = new
                    {
                        IsActive = T[FrameworkWebLocalizableStrings.Membership.UserModel.IsActive].Value,
                        Roles = T[FrameworkWebLocalizableStrings.Membership.UserModel.Roles].Value,
                        UserName = T[FrameworkWebLocalizableStrings.Membership.UserModel.UserName].Value,
                    },
                    Role = new
                    {
                        Name = T[FrameworkWebLocalizableStrings.Membership.RoleModel.Name].Value,
                    },
                    Permission = new
                    {
                        Category = T[FrameworkWebLocalizableStrings.Membership.PermissionModel.Category].Value,
                        Name = T[FrameworkWebLocalizableStrings.Membership.PermissionModel.Name].Value,
                    }
                }
            });
        }
    }
}