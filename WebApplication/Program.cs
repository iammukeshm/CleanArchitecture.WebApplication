using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await Infrastructure.Persistence.Identity.IdentityContextSeed.SeedRolesAsync(userManager, roleManager);
                    await Infrastructure.Persistence.Identity.IdentityContextSeed.SeedAdminAsync(userManager, roleManager);
                    await Infrastructure.Persistence.Identity.IdentityContextSeed.SeedBasicUserAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    //Log.Warning(ex, "An error occurred seeding the DB");
                }
                finally
                {
                    //Log.CloseAndFlush();
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
