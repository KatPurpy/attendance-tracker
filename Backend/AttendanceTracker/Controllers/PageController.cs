using AttendanceTracker.Models;
using AttendanceTracker.Models.Edit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AttendanceTracker.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<PageController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDatabaseContext dbCtx;

        public PageController(AppDatabaseContext dbCtx, ILogger<PageController> logger, UserManager<IdentityUser> userManager)
        {
            this.dbCtx = dbCtx;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Group")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Teacher")]
		public async Task<IActionResult> Group()
		{
			ViewBag.DbCtx = dbCtx;
			ViewBag.User = await GetCurrentUserIdentity();
			return View();
		}

		private async Task<IdentityUser> GetCurrentUserIdentity()
		{
			return await _userManager.GetUserAsync(User);
		}

		[HttpGet]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		[Route("GroupStudent/{id}")]
        [Authorize(Roles = "Teacher")]
		public async Task<IActionResult> GroupStudent(int id)
		{
			ViewBag.DbCtx = dbCtx;
            var entry = dbCtx.Groups.Find(id);

			if (entry == null || !OwnershipUtils.DoesUserHaveAccess(await GetCurrentUserIdentity(), entry))
            {
                return Forbid();
            }

			dbCtx.Entry(entry).Collection(t=>t.Students).Load();
			return View(
                new StudentListEditModel()
                {
                    Group = entry
				}
                );
		}

        [Authorize(Roles = "Teacher")]
        [HttpGet]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		[Route("TimeTable/{id}")]
		public async Task<IActionResult> TimeTable(int id, DateTime date)
		{
            
			ViewBag.DbCtx = dbCtx;
			var entry = dbCtx.Groups.Find(id);
			
            if(entry == null || !OwnershipUtils.DoesUserHaveAccess(await GetCurrentUserIdentity(), entry))
            {
                return Forbid();
            }

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