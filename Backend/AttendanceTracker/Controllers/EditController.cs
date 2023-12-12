using AttendanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AttendanceTracker.Controllers
{
    public class EditController : Controller
    {
        private readonly ILogger<EditController> _logger;

        private readonly DbCtx dbCtx;

        public EditController(DbCtx dbCtx, ILogger<EditController> logger)
        {
            this.dbCtx = dbCtx;
            _logger = logger;
        }

        public IActionResult Group()
        {
            ViewBag.DbCtx = dbCtx;
            return View();
        }

        [Route("GroupStudent/{id}")]
		public IActionResult GroupStudent(int id)
		{
			ViewBag.DbCtx = dbCtx;
            ViewBag.Group = dbCtx.Groups.Find(id);
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}