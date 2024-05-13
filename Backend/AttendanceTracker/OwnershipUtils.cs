using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Identity;

namespace AttendanceTracker
{
	public class OwnershipUtils
	{
		public static bool DoesUserHaveAccess(IdentityUser user, Group group)
		{
			Console.WriteLine("USER {0} GROUP {1}", user.Id, group.OwnerId);
			return group.OwnerId == user.Id;
		}

		public static bool DoesUserHaveAccess(IdentityUser user, AppDatabaseContext context, Student student)
		{
			context.Entry(student).Reference(e => e.Group).LoadAsync().Wait();
			return DoesUserHaveAccess(user, student.Group);
		}

		public static bool DoesUserHaveAccess(IdentityUser user, AppDatabaseContext context, DayEntry dayEntry)
		{
			context.Entry(dayEntry).Reference(e => e.Student).LoadAsync().Wait();
			return DoesUserHaveAccess(user, context, dayEntry.Student);
		}
	}
}
