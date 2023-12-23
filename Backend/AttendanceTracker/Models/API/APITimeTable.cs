using AttendanceTracker.Models.DB;

namespace AttendanceTracker.Models.API
{
	public class APITimeTable
	{
		public List<APIDayEntry> DayEntries { get; set; } = new();

		public void AddEntry(DbCtx context, DayEntry entry)
		{
			DayEntries.Add(new APIDayEntry().ConvertToAPI(context, entry));
		}
	}
}
