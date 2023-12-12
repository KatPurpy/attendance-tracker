using AttendanceTracker.Models;
using AttendanceTracker.Models.Edit;
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

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Group()
        {
            ViewBag.DbCtx = dbCtx;
            return View();
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		[Route("GroupStudent/{id}")]
		public IActionResult GroupStudent(int id)
		{
			ViewBag.DbCtx = dbCtx;
            var entry = dbCtx.Groups.Find(id);
            dbCtx.Entry(entry).Collection(t=>t.Students).Load();
			return View(
                new StudentListEditModel()
                {
                    Group = entry
				}
                );
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}