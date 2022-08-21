using Microsoft.EntityFrameworkCore;

namespace OngProject.DataAccess
{
    public class AppDbContext : DbContext
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
