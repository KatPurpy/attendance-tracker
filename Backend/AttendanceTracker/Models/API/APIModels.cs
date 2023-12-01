using AttendanceTracker.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AttendanceTracker.Models.API
{
    public class APIGroup : IAPIModelFor<APIGroup, DB.Group>, IStringNameDbKey
    {
        public string Name{ get; set; }
        public APIGroup ConvertToAPI(DbCtx databaseContext, DB.Group entity)
        {
            Name = entity.Name;
            return this;
        }
    }

    public class APIStudent : IAPIModelFor<APIStudent, DB.Student>, IGuidDbKey
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }

        public APIStudent ConvertToAPI(DbCtx databaseContext, DB.Student entity)
        {
            Guid = entity.Guid;
            Name = entity.Name;
            GroupName = entity.GroupName;
            return this;
        }
    }

    public class APIDayEntry : IAPIModelFor<APIDayEntry, DB.DayEntry>, IGuidDbKey
    {
        public Guid Guid { get; set; }
        public DateTime Timestamp { get; set; }
        [MaxLength(16)]
        public string Value { get; set; }
        public Guid StudentId { get; set; }

        public APIDayEntry ConvertToAPI(DbCtx databaseContext, DayEntry entity)
        {
            Guid = entity.Guid;
            Timestamp = entity.Timestamp;
            Value = entity.Value;
            StudentId = databaseContext.Set<Student>().First(ent => ent.Guid == entity.Guid).Guid;
            return this;
        }
    }
}
