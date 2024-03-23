using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize]
    public class StudentController : BaseObjectListController<Student, APIStudent>
    {
        public StudentController(DbCtx context) : base(context)
        {

        }
    }
}
