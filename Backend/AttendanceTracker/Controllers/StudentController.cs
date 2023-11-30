using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;

namespace AttendanceTracker.Controllers
{
    public class StudentController : BaseObjectListController<int, Student, APIStudent>
    {
        public StudentController(DbCtx context) : base(context, "Id")
        {

        }
    }
}
