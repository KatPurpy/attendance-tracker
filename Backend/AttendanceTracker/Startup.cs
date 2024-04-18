using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Enviroment { get;}
        public Startup(IConfiguration configuration, IWebHostEnvironment  env)
        {
            Configuration = configuration;
            Enviroment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
 


            // Add services to the container.
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddRazorPages();
            

            services.AddDbContext<DbCtx>((options) =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = !true).AddEntityFrameworkStores<DbCtx>();

            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/Login";
                options.LoginPath = "/Login";
                });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            }
            );

            if (this.Enviroment.IsDevelopment())
            {
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/error/notfound";
                    await next();
                }
            });


            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            // Seed for demo
            using (var scope = app.ApplicationServices.CreateScope())
            {
                DemoSeed(scope.ServiceProvider).Wait();
            }

			app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new
                    {
                        controller = "Home",
                        action = "Index",
                    }
                );

            });

        }

        static async Task DemoSeed(IServiceProvider services)
        {
			var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbctx = services.GetRequiredService<DbCtx>();

                {
                    dbctx.Database.EnsureCreated();

                    var migrations = await dbctx.Database.GetPendingMigrationsAsync();

                    if (migrations.Any())
                    {
                        await dbctx.Database.MigrateAsync();
                    }
                }
                
                
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                

                string[] roles = new[] { "Administrator", "Teacher" };

                foreach (var role in roles)
                {
                    var identityRoleExists = await roleManager.RoleExistsAsync(role);
                    if (!identityRoleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                (IdentityUser user, string[] roles, string password)[] users = new[]
                {
                    (new IdentityUser("administrator"), new string[]{"Administrator"}, "#LcrF2qCyWd5"),
                    (new IdentityUser("michael_ivanovich"), new string[]{"Teacher"},"@oYdXo4c5Lm")
                };

                foreach(var userInfo in users)
                {
                    var user = await userManager.FindByNameAsync(userInfo.user.UserName);
                    if(user == null)
                    {
                        await userManager.CreateAsync(userInfo.user, userInfo.password);
                        user = await userManager.FindByIdAsync(userInfo.user.Id);
                        await userManager.AddToRolesAsync(user, userInfo.roles);
                    }
                }

                await dbctx.SaveChangesAsync();
            }
        }
    }
}
