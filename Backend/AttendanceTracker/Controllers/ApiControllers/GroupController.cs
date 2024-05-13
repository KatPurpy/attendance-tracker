﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Teacher")]
    public class GroupController : BaseObjectListController<Models.DB.Group, Models.API.APIGroup>
    {
        public GroupController(AppDatabaseContext context) : base(context)
        {

        }
    }
}
