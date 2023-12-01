﻿using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;

namespace AttendanceTracker.Controllers.ApiControllers
{
    public class StudentController : BaseObjectListController<Student, APIStudent>
    {
        public StudentController(DbCtx context) : base(context)
        {

        }
    }
}
