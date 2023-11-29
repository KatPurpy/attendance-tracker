using AttendanceTracker.Models;
using Microsoft.EntityFrameworkCore;
namespace AttendanceTracker
{
    public class DbCtx : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<DayEntry> DayEntries { get; set; }

        public DbCtx(DbContextOptions<DbCtx> options) : base(options) { }
    }
}
