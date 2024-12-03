using Microsoft.EntityFrameworkCore;
using ProniaApplication.DAL;

namespace ProniaApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDBContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))

                );

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute(
               name: "admin",
               pattern: "{area:exists}/{controller=home}/{action=index}/{id?}"
               );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}"
                );

            app.Run();
        }
    }
}
