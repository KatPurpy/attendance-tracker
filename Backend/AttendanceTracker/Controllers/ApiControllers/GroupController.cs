using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Teacher")]
    public class GroupController : BaseObjectListController<Models.DB.Group, Models.API.APIGroup>
    {
        public GroupController(AppDatabaseContext context, UserManager<IdentityUser> usermanager) : base(context, usermanager)
        {

        }

		public override bool UserHasAccess(IdentityUser user, AppDatabaseContext context, Group entry)
		{
			return OwnershipUtils.DoesUserHaveAccess(user, entry);
		}

		public override Task AssignUserId(IdentityUser user, Group entry)
		{
			entry.OwnerId = user.Id; 
			return Task.CompletedTask;
		}
	}
}
