using AttendanceTracker;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers
{
    public class GroupController : BaseObjectListController<string, Models.DB.Group, Models.API.APIGroup>
    {
        public GroupController(DbCtx context) : base(context, "Name")
        {

        }
    }
}
