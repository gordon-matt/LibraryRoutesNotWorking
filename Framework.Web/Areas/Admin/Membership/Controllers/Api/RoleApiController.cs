using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extenso.Collections;
using Framework.Infrastructure;
using Framework.Security.Membership;
using Framework.Threading;
using Framework.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Framework.Web.Areas.Admin.Membership.Controllers.Api
{
    public class RoleApiController : ODataController
    {
        private readonly ILogger logger;
        private readonly IWorkContext workContext;

        protected IMembershipService Service { get; private set; }

        public RoleApiController(
            IMembershipService service,
            ILoggerFactory loggerFactory,
            IWorkContext workContext)
        {
            this.Service = service;
            this.logger = loggerFactory.CreateLogger<RoleApiController>();
            this.workContext = workContext;
        }

        public virtual async Task<IActionResult> Get(ODataQueryOptions<FrameworkRole> options)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesRead))
            {
                return Unauthorized();
            }

            //var settings = new ODataValidationSettings()
            //{
            //    AllowedQueryOptions = AllowedQueryOptions.All
            //};
            //options.Validate(settings);

            var results = options.ApplyTo((await Service.GetAllRoles(workContext.CurrentTenant.Id)).AsQueryable());
            var response = await Task.FromResult((results as IQueryable<FrameworkRole>).ToHashSet());
            return Ok(response);
        }

        [EnableQuery]
        public virtual async Task<IActionResult> Get([FromODataUri] string key)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesRead))
            {
                return Unauthorized();
            }

            var entity = await Service.GetRoleById(key);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        public virtual async Task<IActionResult> Put([FromODataUri] string key, [FromBody] FrameworkRole entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesWrite))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(entity.Id))
            {
                return BadRequest();
            }

            try
            {
                await Service.UpdateRole(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                logger.LogError(new EventId(), x, x.Message);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public virtual async Task<IActionResult> Post([FromBody] FrameworkRole entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesWrite))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            entity.TenantId = workContext.CurrentTenant.Id;
            await Service.InsertRole(entity);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IActionResult> Patch([FromODataUri] string key, Delta<FrameworkRole> patch)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesWrite))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FrameworkRole entity = await Service.GetRoleById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                await Service.UpdateRole(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                logger.LogError(new EventId(), x, x.Message);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public virtual async Task<IActionResult> Delete([FromODataUri] string key)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesWrite))
            {
                return Unauthorized();
            }

            FrameworkRole entity = await Service.GetRoleById(key);
            if (entity == null)
            {
                return NotFound();
            }

            await Service.DeleteRole(key);

            return NoContent();
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetRolesForUser([FromODataUri] string userId)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesRead))
            {
                return Unauthorized();
            }

            var results = (await Service.GetRolesForUser(userId)).Select(x => new EdmRole
            {
                Id = x.Id,
                Name = x.Name
            });

            var response = await Task.FromResult(results);
            return Ok(response);
        }

        [HttpPost]
        public virtual async Task<IActionResult> AssignPermissionsToRole([FromBody] ODataActionParameters parameters)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipRolesWrite))
            {
                return Unauthorized();
            }

            string roleId = (string)parameters["roleId"];
            var permissionIds = (IEnumerable<string>)parameters["permissions"];

            await Service.AssignPermissionsToRole(roleId, permissionIds);

            return Ok();
        }

        protected virtual bool EntityExists(string key)
        {
            return AsyncHelper.RunSync(() => Service.GetUserById(key)) != null;
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }

    public struct EdmRole
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}