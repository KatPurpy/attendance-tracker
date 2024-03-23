using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace AttendanceTracker
{
    public class DbCtx : IdentityDbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<DayEntry> DayEntries { get; set; }

        public DbSet<RecycleBinGroupEntry> RecycleBinGroups { get; set; }
        public DbSet<RecycleBinStudentEntry> RecycleBinStudents { get; set; }

        public DbCtx(DbContextOptions<DbCtx> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Seed admin
            base.OnModelCreating(modelBuilder);;
        }
    }
}
