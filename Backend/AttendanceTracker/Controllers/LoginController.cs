using AttendanceTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers
{
	public class LoginController : Controller
	{
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

		public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp([FromForm] LoginModel login)
		{
            if(ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser()
                {
                    UserName = login.Email,
                    Email = login.Email
                };
                
                var result = await _userManager.CreateAsync(user, login.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Content("Success!");
                }
                else
                {
                    
                    return Json(result.Errors);
                }
            }
            else
            {
                return Content("Invalid state");
            }
            return Content("Sign in error.");
		}

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Redirect("/Group");
            }
            else
            {
                return Content("login error");
            }
        }
    }
}
