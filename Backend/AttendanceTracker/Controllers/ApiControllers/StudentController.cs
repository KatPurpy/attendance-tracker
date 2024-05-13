using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Teacher")]
    public class StudentController : BaseObjectListController<Student, APIStudent>
    {
        public StudentController(AppDatabaseContext context) : base(context)
        {

        }
    }
}
