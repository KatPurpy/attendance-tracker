using AttendanceTracker.Models.DB;

namespace AttendanceTracker.Models.API
{
    public class APIGroup : IAPIModelFor<APIGroup, DB.Group>
    {
        public string Name { get; set; } 
        public APIGroup ConvertToAPI(DB.Group entity)
        {
            Name = entity.Name;
            return this;
        }
    }

    public class APIStudent : IAPIModelFor<APIStudent, DB.Student>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }

        public APIStudent ConvertToAPI(DB.Student entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            GroupName = entity.GroupName;
            return this;
        }
    }
}
