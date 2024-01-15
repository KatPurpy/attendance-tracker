using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceTracker.Models.DB
{
	[Keyless]
	public class RecycleBinGroupEntry : IIntDbKey
	{
		[NotMapped]
		public int Id
		{
			get => GroupId; 
			set
			{
				GroupId = value;
			}
		}
		
		public int GroupId { get; set; }
		public Group Group { get; set; } = null;
		public DateTime ExpiresBy { get; set; }
	}

	[Keyless]
	public class RecycleBinStudentEntry : IIntDbKey
	{
		[NotMapped]
		public int Id
		{
			get => StudentId; 
			set
			{
				StudentId = value;
			}
		}

		public int StudentId { get; set; }
		public Student Student { get; set; } = null;
		public DateTime ExpiresBy { get; set; }
	}
}
