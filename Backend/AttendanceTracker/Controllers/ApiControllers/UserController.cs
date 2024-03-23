using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Controllers.ApiControllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        public UserManager<IdentityUser> UserManager { get; set; }

        public UserController(UserManager<IdentityUser> userManager) 
        { 
            UserManager = userManager;
        }

        public JsonResult GetUsers()
        {
            return Json(UserManager.Users.Select(user => user.Id).ToList());
        }
    }
}
