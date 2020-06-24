using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MapApp
{
    class GEOContext : DbContext
    {
        public DbSet<ObjectItem> ObjectItems { get; set; }
        public DbSet<Point> Points { get; set; }

        public GEOContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        }
    }
}
