using Product_service.Dto;
using Product_service.Domain;

namespace Product_service.Helper
{
    public interface IProductMapper
    {
        public ProductDto EntityToDto(Product product);
        public Product DtoToEntity(ProductDto productDto);
    }
}
