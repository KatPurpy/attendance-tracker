using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize]
    public class GroupController : BaseObjectListController<Models.DB.Group, Models.API.APIGroup>
    {
        public GroupController(DbCtx context) : base(context)
        {

        }
    }
}
