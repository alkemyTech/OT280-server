using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;

namespace OngProject.DataAccess
{
    public class AppDbContext : IdentityDbContext<Users, Roles, string>
    {
        public AppDbContext(DbContextOptions ops) : base(ops)
        {
            
        }
        public DbSet<OngProject.Core.Models.Categories> Categories { get; set; }
        public DbSet<OngProject.Core.Models.Members> Members { get; set; }
        public DbSet<OngProject.Core.Models.News> News { get; set; }
        public DbSet<OngProject.Core.Models.Organization> Organization { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            Users user = new Users()
            {
                Id = "1",
                UserName = "Admin",
                Email = "admin@gmail.com",
                //LockoutEnabled = false,
                PhoneNumber = "1234567890",
                FirstName = "Diego",
                LastName = "Balde",
                Password = "Password@123"
            };

            PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
            passwordHasher.HashPassword(user, "Admin*123");

            builder.Entity<Users>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Roles>().HasData(
                new Roles() { Id = "1", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new Roles() { Id = "2", Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "1", UserId = "1" },
                new IdentityUserRole<string>() { RoleId = "2", UserId = "1" }
                );
        }
    }
}
