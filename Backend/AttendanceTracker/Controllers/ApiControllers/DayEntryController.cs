using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers.ApiControllers
{
    public class DayEntryController : BaseObjectListController<string, Models.DB.DayEntry, Models.API.APIDayEntry>
    {
        public DayEntryController(DbCtx context) : base(context)
        {

        }
    }
}
