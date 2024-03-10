using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Models
{
	public class LoginModel
	{
		[EmailAddress]
		[MaxLength(255)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
