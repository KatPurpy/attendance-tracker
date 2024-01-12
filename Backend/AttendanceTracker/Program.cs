using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
namespace AttendanceTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
            }

            builder.Services.AddDbContext<DbCtx>((options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL") );
            });

            var app = builder.Build();

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
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                });



            app.Run();
        }
    }
}