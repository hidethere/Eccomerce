using Product_service.Domain;
using Product_service.Dto;
using Product_service.Event;

namespace Product_service.Service
{
    public interface IProductService
    {
        public Task<ProductDto> CreateProductAsync(ProductDto productdto);
        public Task<ProductDto> GetProductByIdAsync(Guid productId);
        public Task<ProductDto> SellProductAsync(ProductSoldEvent productSoldEvent);
        public Task<IEnumerable<ProductDto>> GetProductsAsync(int? limit);

    }
}
