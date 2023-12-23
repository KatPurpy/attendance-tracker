using AttendanceTracker.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AttendanceTracker.Models.API
{
    public class APIGroup : IAPIModelFor<APIGroup, DB.Group>, IIntDbKey
    {
        [HiddenInput(DisplayValue = false)]
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
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }
        public string Name { get; set; }
		[HiddenInput(DisplayValue = false)]
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
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }
		public DateTime Timestamp { get; set; }
        [MaxLength(16)]
        public string Value { get; set; }
        public int StudentId { get; set; }

        public APIDayEntry ConvertToAPI(DbCtx databaseContext, DayEntry entity)
        {
            Id = entity.Id;
            Timestamp = entity.Timestamp;
            Value = entity.Value;
            StudentId = entity.StudentId;
            return this;
        }
    }
}
