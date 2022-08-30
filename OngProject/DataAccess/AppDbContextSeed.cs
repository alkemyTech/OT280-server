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
            #region Create Roles

            //Create Roles
            var adminRole = new Roles{ Name = "admin"};
            var standardRole = new Roles { Name = "standard" };

            if (await roleManager.Roles.AllAsync(r => r.Name != adminRole.Name))
                await roleManager.CreateAsync(adminRole);
            
            if (await roleManager.Roles.AllAsync(r => r.Name != standardRole.Name))
                await roleManager.CreateAsync(standardRole);

            #endregion

            #region Create Users

            //Create Users
            for (var i = 1; i <= 10; i++)
            {
                var admin = new Users
                {
                    FirstName = "FirstName " + i,
                    LastName = "LastName " + i,
                    Email = "Email" + i + "@example.com",
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
                    Email = "Email" + i + "@example.com",
                    UserName = "standard"+i,
                    Photo = null,
                };

                if (userManager.Users.All(u => u.UserName != standard.UserName))
                {
                    await userManager.CreateAsync(standard,"Abc123.");
                    await userManager.AddToRoleAsync(standard, standardRole.Name);
                }
            }

            #endregion
            
        }
        public static async Task SeedMembers(AppDbContext context)
        {
            if (!context.Members.Any())
            {
                context.Members.Add(new Members
                {
                    Name = "Juan",
                    FacebookUrl = "juan@facebook",
                    InstagramUrl = "juan@instagram",
                    LinkedinUrl = "juan@linkedin",
                    Description = "Juan es un miembro",
                    Image = ""
                });
                
                context.Members.Add(new Members
                {
                    Name = "Gonzalo",
                    FacebookUrl = "Gonzalo@facebook",
                    InstagramUrl = "Gonzalo@instagram",
                    LinkedinUrl = "Gonzalo@linkedin",
                    Description = "Gonzalo es un miembro",
                    Image = ""
                });
            }
            await context.SaveChangesAsync();
        }

        public static async Task SeedCategories(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Categories
                {
                    Name = "Educación",
                    Description = "Educación",
                    Image = "",
                    //byte[] ChangeCheck 
                    //bool IsDeleted 
                    //ICollection <News> News { get; set; }
                    //consultar si agrego las novedades
                });
                context.Categories.Add(new Categories
                {
                    Name = "Deportes",
                    Description = "Deportes",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Primera infancia",
                    Description = "Primera infancia",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Salud",
                    Description = "Salud",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Alimentación",
                    Description = "Alimentación",
                    Image = "",
                });
                context.Categories.Add(new Categories
                {
                    Name = "Trabajo Social",
                    Description = "Trabajo Social",
                    Image = "",
                });
            }
            await context.SaveChangesAsync();
        }
    }
}