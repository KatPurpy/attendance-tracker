using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
