using Product_service.Domain;

namespace Product_service.Repository
{
    public interface IProductRepository
    {
        public Task<Product> FindProductByIdAsync(Guid productId);
        public Task<Product> FindProductByNameAsync(string name);
        public IQueryable<Product> GetProductsAsync();
        public Task<Product> AddAsync(Product product);
    }
}
