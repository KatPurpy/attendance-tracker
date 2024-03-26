using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Identity;

namespace AttendanceTracker
{
	public class OwnershipUtils
	{
		public static bool DoesUserHaveAccess(IdentityUser user, DbCtx ctx, Student student)
		{
			return ctx.Groups.First(group => group.OwnerId == user.Id).Students.Contains(student);
		}
	}
}
