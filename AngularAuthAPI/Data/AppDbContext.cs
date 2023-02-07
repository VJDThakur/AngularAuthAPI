using AngularAuthAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options)
            : base(options)
        {

        }
        public DbSet <User> user { get; set; } 
    }
}
