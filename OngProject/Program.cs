using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OngProject.Core.Models;
using OngProject.DataAccess;

namespace OngProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    services.GetRequiredService<AppDbContext>().Database.Migrate();

                    var userManager = services.GetRequiredService<UserManager<Users>>();
                    var roleManager = services.GetRequiredService<RoleManager<Roles>>();
                    var context = services.GetRequiredService<AppDbContext>();

                    await AppDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                    await AppDbContextSeed.SeedMembers(context);
                    await AppDbContextSeed.SeedCategories(context);
                    await AppDbContextSeed.SeedActivities(context);

                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "Ocurrio un error al hacer la migracion o la seed de datos");
                    throw;
                }
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}
