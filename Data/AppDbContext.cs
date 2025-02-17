using Microsoft.EntityFrameworkCore;
using UsersAuth.Models;

namespace UsersAuth.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=nita;Password=nita1122;Server=localhost;Port=5432;Database=User");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
