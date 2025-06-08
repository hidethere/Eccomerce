using Microsoft.EntityFrameworkCore;
using Product_service.Domain;

namespace Product_service.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options){ }
            public DbSet<Product> Products { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Product>().ToContainer("Products");
                modelBuilder.Entity<Product>().HasPartitionKey(p => p.CategoryId);
                modelBuilder.Entity<Product>().Property(p => p.Id).ToJsonProperty("id"); //Maps into lower case in Cosmo
                modelBuilder.Entity<Product>().HasKey(p => p.Id);
        }

    }
}
