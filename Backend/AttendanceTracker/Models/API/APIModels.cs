using AttendanceTracker.Models.DB;
using System.Text.Json.Serialization;

namespace AttendanceTracker.Models.API
{
    public class APIGroup : IAPIModelFor<APIGroup, DB.Group>, IStringNameDbKey
    {
        public string Name{ get; set; }
        public APIGroup ConvertToAPI(DB.Group entity)
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

        public APIStudent ConvertToAPI(DB.Student entity)
        {
            Guid = entity.Guid;
            Name = entity.Name;
            GroupName = entity.GroupName;
            return this;
        }
    }
}
