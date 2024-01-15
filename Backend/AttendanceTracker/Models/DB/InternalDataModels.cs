namespace AttendanceTracker.Models.DB
{
	public class RecycleBinGroupEntry : IIntDbKey
	{
		public int Id
		{
			get => GroupId; set
			{
				GroupId = value;
			}
		}
		public int GroupId { get; set; }
		public Group Group { get; set; } = null;
		public DateTime ExpiresBy { get; set; }
	}

	public class RecycleBinStudentEntry : IIntDbKey
	{
		public int Id
		{
			get => StudentId; set
			{
				StudentId = value;
			}
		}
		public int StudentId { get; set; }
		public Student Student { get; set; } = null;
		public DateTime ExpiresBy { get; set; }
	}
}
