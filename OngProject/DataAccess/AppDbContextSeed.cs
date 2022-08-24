using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;

namespace OngProject.DataAccess
{
    public static class AppDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<Users> userManager,
            RoleManager<Roles> roleManager)
        {
            //Create Roles
            var adminRole = new Roles{ Name = "admin"};
            var standardRole = new Roles { Name = "standard" };

            if (await roleManager.Roles.AllAsync(r => r.Name != adminRole.Name))
                await roleManager.CreateAsync(adminRole);
            
            if (await roleManager.Roles.AllAsync(r => r.Name != standardRole.Name))
                await roleManager.CreateAsync(standardRole);
            
            //Create Users
            for (var i = 1; i <= 10; i++)
            {
                var admin = new Users
                {
                    FirstName = "FirstName " + i,
                    LastName = "LastName " + i,
                    Email = "Email" + i + "example.com",
                    UserName = "admin"+i,
                    Photo = null,
                };

                if (userManager.Users.All(u => u.UserName != admin.UserName))
                {
                    await userManager.CreateAsync(admin,"Abc123.");
                    await userManager.AddToRoleAsync(admin, adminRole.Name);
                }
            }
            
            for (var i = 1; i <= 10; i++)
            {
                var standard = new Users
                {
                    FirstName = "FirstName " + i,
                    LastName = "LastName " + i,
                    Email = "Email" + i + "example.com",
                    UserName = "standard"+i,
                    Photo = null,
                };

                if (userManager.Users.All(u => u.UserName != standard.UserName))
                {
                    await userManager.CreateAsync(standard,"Abc123.");
                    await userManager.AddToRoleAsync(standard, standardRole.Name);
                }
            }
            
        }
        
    }
}