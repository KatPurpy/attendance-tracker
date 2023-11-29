using System.ComponentModel;
namespace AttendanceTracker.Models
{
    public class UserProfileModel
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
