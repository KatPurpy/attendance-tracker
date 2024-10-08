﻿using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Teacher")]
    public class DayEntryController : BaseCRUD<Models.DB.DayEntry, Models.API.APIDayEntry>
    {
        public DayEntryController(AppDatabaseContext context, UserManager<IdentityUser> usermanager) : base(context, usermanager)
        {

        }

        [HttpGet]
        [Route("GetEntries/{groupID}")]
        public async Task<ActionResult<APITimeTable>> GetEntries(int groupID, DateTime date)
        {
            var group = await DbCtx.Groups.FindAsync(groupID);
            if( group == null)
            {
                return NotFound();
            }
			int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
			DateTime rangeStart = new DateTime(date.Year, date.Month, 1, 0,0,0,DateTimeKind.Utc);
			DateTime rangeEnd = new DateTime(date.Year, date.Month, daysInMonth, 0, 0, 0, DateTimeKind.Utc);


			var groupStudents = DbCtx.Students.Where(student => student.GroupId == groupID).Select(student => student.Id);
            
            var dayEntries = DbCtx.DayEntries.Where(
                e => groupStudents.Contains(e.StudentId) 
                && rangeStart <= e.Timestamp
                && e.Timestamp <= rangeEnd);


            var timeTable = new APITimeTable();

            foreach (var entry in dayEntries)
            {
                timeTable.AddEntry(DbCtx,entry);
            }

            return timeTable;

		}

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int studentID, DateTime day, string value)
        {
            var student = await DbCtx.Students.FindAsync(studentID);
            if(student == null) { return NoContent(); }
            
            // quantize day datetime to days
            day = new DateTime(day.Year,day.Month,day.Day,0,0,0,DateTimeKind.Utc);

            DayEntry? entry = null;

			entry = DbCtx.DayEntries.FirstOrDefault(e => (e.Timestamp == day) && (e.StudentId == studentID));

            if (entry != null)
            {
                if (value == null)
                {
                    DbCtx.Remove(entry);
				}
                else
                {

                    entry.Timestamp = day;
                    entry.Value = value;
                }
			}
            else
            {
                entry = new DayEntry()
                {
                    Id = default,
                    StudentId = studentID,
                    Timestamp = day,
                    Value = value
                };
                DbCtx.DayEntries.Add(entry);
			}

            await DbCtx.SaveChangesAsync();
            return Ok();
        }

		public override bool UserHasAccess(IdentityUser user, AppDatabaseContext context, DayEntry entry)
		{
			return OwnershipUtils.DoesUserHaveAccess(user, context, entry);
		}
	}
}
