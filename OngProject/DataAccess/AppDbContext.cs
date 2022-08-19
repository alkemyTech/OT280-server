using Microsoft.EntityFrameworkCore;

namespace OngProject.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions ops) : base(ops)
        {
            
        }
    }
}
