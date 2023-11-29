﻿using System.ComponentModel.DataAnnotations;
namespace AttendanceTracker.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupId { get; set; }
        public Group Group { get; set; } = null;
    }

    public class Group
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set;} = new List<Student>();
    }

    public class DayEntry
    {
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null;
    }
}
