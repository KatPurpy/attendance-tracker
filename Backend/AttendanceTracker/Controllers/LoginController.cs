using AttendanceTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult<LoginModel> SignUp([FromForm] LoginModel login)
		{
			Console.WriteLine("AAA");
			var a = Request.Form;
			return login;
		}

        [HttpPost]
        public ActionResult<LoginModel> Login([FromForm] LoginModel model)
        {
            return model;
        }
    }
}
