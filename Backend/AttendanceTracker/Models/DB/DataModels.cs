using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceTracker.Models.DB
{
    public class Student : IGuid
    {
        [Key]
        public int Id { get; set; }
        
        public Guid Guid { get; set; } = new Guid();
        public string Name { get; set; }
        public string GroupName { get; set; }

        public Group Group { get; set; } = null;
    }

    public class Group
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }

    [PrimaryKey(nameof(Id), nameof(Guid))]
    public class DayEntry
    {
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        public DateTime Timestamp { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null;
    }
}
