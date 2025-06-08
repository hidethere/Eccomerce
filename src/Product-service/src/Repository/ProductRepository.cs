using Microsoft.EntityFrameworkCore;
using Product_service.Domain;
using Product_service.Persistence;

namespace Product_service.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _appDbContext;
        public ProductRepository(ProductDbContext context) 
        { 
            _appDbContext = context;
        }
        public async Task<Product> AddAsync(Product product)
        {
           await  _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            
            return product;
        }

        public async Task<Product> FindProductByIdAsync(Guid productId)
        {
            return await _appDbContext.Products.FindAsync(productId);
        }

        public async Task<Product> FindProductByNameAsync(string name)
        {
            return await _appDbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public IQueryable<Product> GetProductsAsync()
        {
            return _appDbContext.Products.AsQueryable();
        }
    }
}
