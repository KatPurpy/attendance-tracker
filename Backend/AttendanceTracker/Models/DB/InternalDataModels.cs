using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceTracker.Models.DB
{

	public class RecycleBinGroupEntry
	{
		[Key]
		[ForeignKey(nameof(Group))]
		public int GroupId { get; set; }
		public Group Group { get; set; } = null;
		public DateTime ExpiresBy { get; set; }
	}

	public class RecycleBinStudentEntry
	{
		[Key]
		[ForeignKey(nameof(Student))]
		public int StudentId { get; set; }
		public Student Student { get; set; } = null;
		public DateTime ExpiresBy { get; set; }
	}
}
