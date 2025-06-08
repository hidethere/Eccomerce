using Microsoft.EntityFrameworkCore;
using Product_service.Domain;

namespace Product_service.Persistence
{
    public class CosmoDbContext : DbContext
    {
        public CosmoDbContext(DbContextOptions<CosmoDbContext> options) : base(options) { }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("ProductContainer");

            modelBuilder.Entity<Product>().ToContainer("Products");
            modelBuilder.Entity<Product>().HasPartitionKey(p => p.CategoryId);
            modelBuilder.Entity<Product>().Property(p => p.Id).ToJsonProperty("id"); //Maps into lower case in Cosmo
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Category>().ToContainer("Categories");
            modelBuilder.Entity<Category>().HasPartitionKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).ToJsonProperty("id");
            modelBuilder.Entity<Category>().HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);

        }
    }
}
