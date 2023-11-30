using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers.ApiControllers
{
    public class GroupController : BaseObjectListController<string, Models.DB.Group, Models.API.APIGroup>
    {
        public GroupController(DbCtx context) : base(context, "Name")
        {

        }
    }
}
