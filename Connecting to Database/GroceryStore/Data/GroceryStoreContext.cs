using GroceryStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStore.Data
{
    public class GroceryStoreContext : DbContext
    {
        public DbSet<GroceryItem> GroceryTable { get; set; }
        public GroceryStoreContext(DbContextOptions<GroceryStoreContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryItem>().HasData(
                new GroceryItem
                {
                    ItemID = 1,
                    ItemName = "Carrot",
                    ItemPrice = 4.50M
                },
                new GroceryItem
                {
                    ItemID = 2,
                    ItemName = "Banana",
                    ItemPrice = 3.50M
                },
                new GroceryItem
                {
                    ItemID = 3,
                    ItemName = "Lemon",
                    ItemPrice = 0.99M
                }
            );
        }
    }
}
