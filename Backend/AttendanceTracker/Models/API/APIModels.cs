using AttendanceTracker.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AttendanceTracker.Models.API
{
    public class APIGroup : IAPIModelFor<APIGroup, DB.Group>, IIntDbKey
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public APIGroup ConvertToAPI(DbCtx databaseContext, DB.Group entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            return this;
        }
    }

    public class APIStudent : IAPIModelFor<APIStudent, DB.Student>, IIntDbKey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }

        public APIStudent ConvertToAPI(DbCtx databaseContext, DB.Student entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            GroupId = entity.GroupId;
            return this;
        }
    }

    public class APIDayEntry : IAPIModelFor<APIDayEntry, DB.DayEntry>, IIntDbKey
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        [MaxLength(16)]
        public string Value { get; set; }
        public Guid StudentId { get; set; }

        public APIDayEntry ConvertToAPI(DbCtx databaseContext, DayEntry entity)
        {
            Id = entity.Id;
            Timestamp = entity.Timestamp;
            Value = entity.Value;
            StudentId = databaseContext.Set<Student>().First(ent => ent.Guid == entity.Guid).Guid;
            return this;
        }
    }
}
