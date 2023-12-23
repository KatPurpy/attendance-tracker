using AttendanceTracker.Models.API;
using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Controllers.ApiControllers
{
    public class DayEntryController : BaseObjectListController<Models.DB.DayEntry, Models.API.APIDayEntry>
    {
        public DayEntryController(DbCtx context) : base(context)
        {

        }

        [HttpGet]
        [Route("GetEntries/{groupID}")]
        public async Task<ActionResult<APITimeTable>> GetEntries(int groupID, DateTime rangeStart, DateTime rangeEnd)
        {
            var group = await DbCtx.Groups.FindAsync(groupID);
            if( group == null)
            {
                return NotFound();
            }
            rangeStart = new DateTime(rangeStart.Year, rangeStart.Month,  rangeStart.Day, 0, 0, 0, DateTimeKind.Utc);
            rangeEnd = new DateTime(rangeEnd.Year, rangeEnd.Month, rangeEnd.Day, 0, 0, 0, DateTimeKind.Utc);

			var groupStudents = DbCtx.Students.Where(student => student.GroupId == groupID).Select(student => student.Id).ToHashSet();
            var dayEntries = DbCtx.DayEntries.Where(
                e => groupStudents.Contains(e.StudentId) 
                && rangeStart <= e.Timestamp
                && e.Timestamp <= rangeEnd);

            Dictionary<int, List<APIDayEntry>> entries = new Dictionary<int, List<APIDayEntry>>();

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
            if(student == null) { return NotFound(); }
            
            // quantize day datetime to days
            day = new DateTime(day.Year,day.Month,day.Day,0,0,0,DateTimeKind.Utc);

            DayEntry? entry = null;

			entry = DbCtx.DayEntries.Where(e => (e.Timestamp == day) && (e.StudentId == studentID)).FirstOrDefault();

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
				Console.WriteLine("NEW {0} {1} {2}", studentID, day, value);
			}

            await DbCtx.SaveChangesAsync();
            return Ok();
        }
    }
}
