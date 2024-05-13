using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers
{
    public class ApiController : Controller
    {
        public AppDatabaseContext Context;
        public ApiController(AppDatabaseContext context) 
        { 
            Context = context;
        }

        public ActionResult<string> TEST(string hm)
        {
            return new ActionResult<string>("VAL VAL VAL " + hm);
        }
    }
}
