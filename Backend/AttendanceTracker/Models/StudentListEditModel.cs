using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;

namespace AttendanceTracker.Models.Edit
{
	public class StudentListEditModel
	{
		public Group Group;
		public APIStudent CurrentStudent = new();
	}
}
