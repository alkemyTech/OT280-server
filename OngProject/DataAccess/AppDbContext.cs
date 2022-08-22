using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OngProject.Entities.Domain;

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
    }
}
