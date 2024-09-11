// Data/EskitechContext.cs
using Microsoft.EntityFrameworkCore;

namespace EskitechAPI.Data
{
    public class EskitechContext : DbContext
    {
        public EskitechContext(DbContextOptions<EskitechContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } 
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Price> Prices { get; set; }
    }
}