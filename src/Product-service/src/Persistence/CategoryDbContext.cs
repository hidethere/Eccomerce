using Microsoft.EntityFrameworkCore;
using Product_service.Domain;

namespace Product_service.Persistence
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options) { }


        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("ProductContainer");

            
            modelBuilder.Entity<Category>().ToContainer("Categories");
            modelBuilder.Entity<Category>().HasPartitionKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).ToJsonProperty("id");
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);

        }
    }
}
