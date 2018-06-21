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
    public class PermissionApiController : ODataController
    {
        private readonly ILogger logger;
        private readonly IWorkContext workContext;

        protected IMembershipService Service { get; private set; }

        public PermissionApiController(
            IMembershipService service,
            ILoggerFactory loggerFactory,
            IWorkContext workContext)
        {
            this.Service = service;
            this.logger = loggerFactory.CreateLogger<PermissionApiController>();
            this.workContext = workContext;
        }

        public virtual async Task<IActionResult> Get(ODataQueryOptions<FrameworkPermission> options)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsRead))
            {
                return Unauthorized();
            }

            var results = options.ApplyTo(
                (await Service.GetAllPermissions(workContext.CurrentTenant.Id)).AsQueryable());

            var response = await Task.FromResult((results as IQueryable<FrameworkPermission>).ToHashSet());
            return Ok(response);
        }

        [EnableQuery]
        public virtual async Task<IActionResult> Get([FromODataUri] string key)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsRead))
            {
                return Unauthorized();
            }

            var entity = await Service.GetPermissionById(key);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        public virtual async Task<IActionResult> Put([FromODataUri] string key, [FromBody] FrameworkPermission entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsWrite))
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
                await Service.UpdatePermission(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                logger.LogError(new EventId(), x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public virtual async Task<IActionResult> Post([FromBody] FrameworkPermission entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsWrite))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            entity.TenantId = workContext.CurrentTenant.Id;
            await Service.InsertPermission(entity);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IActionResult> Patch([FromODataUri] string key, Delta<FrameworkPermission> patch)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsWrite))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FrameworkPermission entity = await Service.GetPermissionById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                await Service.UpdatePermission(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                logger.LogError(new EventId(), x.Message, x);

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
            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsWrite))
            {
                return Unauthorized();
            }

            FrameworkPermission entity = await Service.GetPermissionById(key);
            if (entity == null)
            {
                return NotFound();
            }

            await Service.DeletePermission(key);

            return NoContent();
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetPermissionsForRole([FromODataUri] string roleId)
        {
            if (!CheckPermission(FrameworkWebPermissions.MembershipPermissionsRead))
            {
                return Unauthorized();
            }

            var role = await Service.GetRoleById(roleId);
            var results = (await Service.GetPermissionsForRole(workContext.CurrentTenant.Id, role.Name)).Select(x => new EdmFrameworkPermission
            {
                Id = x.Id,
                Name = x.Name
            });

            var response = await Task.FromResult(results);
            return Ok(response);
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

    public struct EdmFrameworkPermission
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}