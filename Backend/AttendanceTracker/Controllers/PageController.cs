using AttendanceTracker.Models;
using AttendanceTracker.Models.Edit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AttendanceTracker.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<PageController> _logger;

        private readonly DbCtx dbCtx;

        public PageController(DbCtx dbCtx, ILogger<PageController> logger)
        {
            this.dbCtx = dbCtx;
            _logger = logger;
        }

        [HttpGet]
        [Route("Group")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Group()
        {
            ViewBag.DbCtx = dbCtx;
            return View();
        }

        [HttpGet]
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

		[HttpGet]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		[Route("TimeTable/{id}")]
		public IActionResult TimeTable(int id, DateTime date)
		{
			ViewBag.DbCtx = dbCtx;
			var entry = dbCtx.Groups.Find(id);
			dbCtx.Entry(entry).Collection(t => t.Students).Load();
			
            return View(
				new TimeTableViewModel()
                {
                    group = entry,
                    date = date
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