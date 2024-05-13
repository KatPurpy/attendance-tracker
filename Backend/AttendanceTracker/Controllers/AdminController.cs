using AttendanceTracker.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AttendanceTracker.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public UserManager<IdentityUser> UserManager { get; set; }
        public AppDatabaseContext DbCtx { get; set; }

        public AdminController(UserManager<IdentityUser> userManager, AppDatabaseContext dbCtx)
        {
            UserManager = userManager;
            DbCtx = dbCtx;
        }

        public IActionResult Index()
        {
            DbCtx.Users.Load();
            return View(DbCtx.Users.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool userExists = await UserManager.FindByNameAsync(model.UserName) != null;
            if (!userExists)
            {
                var passwordErrors = new List<IdentityError>();
                foreach (var pv in UserManager.PasswordValidators)
                {
                    var passwordGood = await pv.ValidateAsync(UserManager, null, model.Password);
                    passwordErrors.AddRange(passwordGood.Errors);
                }
                if (passwordErrors.Count > 0)
                {
                    var errors = passwordErrors.Select(pw => pw.Description);
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(nameof(model.Password), error);
                    }
                    return View();
                }

                IdentityUser user = new IdentityUser(model.UserName);
                user.Email = model.Email;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "Teacher");
                    await DbCtx.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("UserName", "User with that name already exists");
            }
            return View();
        }

        public JsonResult GetUsers()
        {
            return Json(UserManager.Users.Select(user => user.Id).ToList());
        }
    }
}
