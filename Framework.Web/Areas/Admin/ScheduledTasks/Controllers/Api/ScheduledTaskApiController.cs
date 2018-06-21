using System;
using System.Threading.Tasks;
using Extenso.Data.Entity;
using Framework.Tasks.Domain;
using Framework.Web.OData;
using Framework.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FrameworkTask = Framework.Tasks.Task;

namespace Framework.Web.Areas.Admin.ScheduledTasks.Controllers.Api
{
    public class ScheduledTaskApiController : GenericODataController<ScheduledTask, int>
    {
        public ScheduledTaskApiController(IRepository<ScheduledTask> repository)
            : base(repository)
        {
        }

        protected override int GetId(ScheduledTask entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(ScheduledTask entity)
        {
            // Do nothing (int is auto incremented)
        }

        public override async Task<IActionResult> Put(int key, ScheduledTask entity)
        {
            var existingEntity = await Service.FindOneAsync(key);
            existingEntity.Seconds = entity.Seconds;
            existingEntity.Enabled = entity.Enabled;
            existingEntity.StopOnError = entity.StopOnError;
            return await base.Put(key, existingEntity);
        }

        [HttpPost]
        public async Task<IActionResult> RunNow([FromBody] ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            int taskId = (int)parameters["taskId"];

            var scheduleTask = await Service.FindOneAsync(taskId);
            if (scheduleTask == null)
                return NotFound();

            var task = new FrameworkTask(scheduleTask);
            //ensure that the task is enabled
            task.Enabled = true;

            try
            {
                task.Execute(true);
            }
            catch (Exception x)
            {
                Logger.LogError(new EventId(), x, x.Message);
                return StatusCode(500, x);
            }

            return Ok();
        }

        protected override Permission ReadPermission
        {
            get { return FrameworkWebPermissions.ScheduledTasksRead; }
        }

        protected override Permission WritePermission
        {
            get { return FrameworkWebPermissions.ScheduledTasksWrite; }
        }
    }
}