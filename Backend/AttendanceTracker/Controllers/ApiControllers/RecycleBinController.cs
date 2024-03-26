using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Administrator, Teacher")]
    [Route("api")]
	public class RecycleBinController : Controller
	{
		DbCtx DbCtx;

		public RecycleBinController(DbCtx dbCtx)
		{
			DbCtx = dbCtx;
		}

		DateTime CalculateExpirationDay(DateTime dateTime)
		{
			return dateTime.AddDays(2);
		}

		[HttpPost]
		[Route("Group/Archive/{id}")]
		public async Task<IActionResult> ArchiveGroup(int id)
		{
			Group? group = await DbCtx.Groups.FindAsync(id);
			if(group != null)
			{
				var grp = DbCtx.RecycleBinGroups;
				var groupBinEntry = await grp.FirstOrDefaultAsync(entry => entry.GroupId == id);
				if (groupBinEntry == null)
				{
					await DbCtx.RecycleBinGroups.AddAsync(new RecycleBinGroupEntry()
					{
						GroupId = id,
						ExpiresBy = CalculateExpirationDay(DateTime.UtcNow)
					});
					await DbCtx.SaveChangesAsync();
				}
				return Ok();
			}
			return NotFound();
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost]
		[Route("Group/Unarchive/{id}")]
		public async Task<IActionResult> UnarchiveGroup(int id)
		{
			var groupBinEntry = await DbCtx.RecycleBinGroups.FirstOrDefaultAsync(entry => entry.GroupId == id);
			if (groupBinEntry != null)
			{
				DbCtx.RecycleBinGroups.Remove(groupBinEntry);
				await DbCtx.SaveChangesAsync();
				return Ok();
			}
			return NotFound();
		}

		[HttpPost]
		[Route("Student/Archive/{id}")]
		public async Task<IActionResult> ArchiveStudent(int id)
		{
			Student? student = await DbCtx.Students.FindAsync(id);
			if (student != null)
			{
				var groupBinEntry = await DbCtx.RecycleBinStudents.FirstOrDefaultAsync(entry => entry.StudentId == id);
				if (groupBinEntry == null)
				{
					DbCtx.RecycleBinStudents.Add(new RecycleBinStudentEntry()
					{
						StudentId = id,
						ExpiresBy = CalculateExpirationDay(DateTime.UtcNow)
					});
					await DbCtx.SaveChangesAsync();
				}
				return Ok();
			}
			return NotFound();
		}

		[HttpPost]
		[Route("Student/Unarchive/{id}")]
		public async Task<IActionResult> UnarchiveStudent(int id)
		{
			var groupBinStudents = await DbCtx.RecycleBinStudents.FirstOrDefaultAsync(entry => entry.StudentId == id);
			if (groupBinStudents != null)
			{
				DbCtx.RecycleBinStudents.Remove(groupBinStudents);
				await DbCtx.SaveChangesAsync();
				return Ok();
			}
			return NotFound();
		}
	}
}