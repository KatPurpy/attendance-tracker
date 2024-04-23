using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Models.Admin
{
    public class CreateTeacherModel
    {
        [Required]
        public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(16, ErrorMessage = "Must be between 6 and 16 characters", MinimumLength = 6)]
		public string Password { get; set; }


		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		[DisplayName("Confirm password")]
		public string ConfirmPassword { get; set; }

	}
}
