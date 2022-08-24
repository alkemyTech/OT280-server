using System.Reflection;
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
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Organization> Organization { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(modelBuilder);
            
            IdentityConfiguration(modelBuilder);
        }
        
        private static void IdentityConfiguration(ModelBuilder builder)
        {
            //Parte en testeo
            
            // builder.Ignore<IdentityUserClaim<string>>();
            // builder.Ignore<IdentityUserLogin<string>>();
            // builder.Ignore<IdentityUserToken<string>>();
            // builder.Ignore<IdentityRoleClaim<string>>();

            builder.Entity<Users>(entity =>
            {
                //entity.ToTable("Users");

                entity.Ignore(u => u.PhoneNumber);
                entity.Ignore(u => u.PhoneNumberConfirmed);
                entity.Ignore(u => u.TwoFactorEnabled);
                entity.Ignore(u => u.LockoutEnd);
                entity.Ignore(u => u.LockoutEnabled);
                entity.Ignore(u => u.AccessFailedCount);
            });

            // builder.Entity<IdentityRole>().ToTable("Roles");
            //
            // builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
