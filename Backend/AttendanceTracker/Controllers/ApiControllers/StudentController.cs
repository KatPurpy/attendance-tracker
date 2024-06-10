using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Teacher")]
    public class StudentController : BaseCRUD<Student, APIStudent>
    {
        public StudentController(AppDatabaseContext context, UserManager<IdentityUser> usermanager) : base(context, usermanager)
        {

        }

		public override bool UserHasAccess(IdentityUser user, AppDatabaseContext context, Student entry)
		{
			return OwnershipUtils.DoesUserHaveAccess(user, context, entry);
		}
	}
}
