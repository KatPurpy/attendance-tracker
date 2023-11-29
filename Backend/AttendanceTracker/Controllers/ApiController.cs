using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers
{
    public class ApiController : Controller
    {
        public DbCtx Context;
        public ApiController(DbCtx context) 
        { 
            Context = context;
        }

        public ActionResult<string> TEST(string hm)
        {
            return new ActionResult<string>("VAL VAL VAL " + hm);
        }
    }
}
