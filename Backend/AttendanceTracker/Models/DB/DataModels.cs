using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AttendanceTracker.Models.DB
{
    public class Student : IGuidDbKey
    {
        [Key]
        public int Id { get; set; }
        
        public Guid Guid { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string GroupName { get; set; }

        public Group Group { get; set; } = null;
    }

    public class Group : IStringNameDbKey
    {
        [Key]
        [MaxLength(64)]
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }

    public class DayEntry : IGuidDbKey
    {
        [Key]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime Timestamp { get; set; }
        [MaxLength(16)]
        public string Value { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null;
    }
}
