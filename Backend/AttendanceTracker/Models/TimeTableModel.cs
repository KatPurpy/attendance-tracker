using AttendanceTracker.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Models
{
	public class TimeTableViewModel
	{
		public Group group;

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime date{ get; set; }

		public IEnumerable<DateTime> GetDaysEnumerator()
		{
			int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
			DateTime RangeStart = new DateTime(date.Year, date.Month, 1);
			DateTime RangeEnd = new DateTime(date.Year, date.Month, daysInMonth);
			for (var day = RangeStart; day <= RangeEnd; day = day.AddDays(1))
			{
				yield return day;
			}
		}
	}
}
