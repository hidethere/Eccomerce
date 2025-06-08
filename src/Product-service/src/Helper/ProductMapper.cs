using Product_service.Dto;
using Product_service.Domain;

namespace Product_service.Helper
{
    public class ProductMapper : IProductMapper
    {
        public Product DtoToEntity(ProductDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
            };
        }

        public ProductDto EntityToDto(Product product)
        {
            return new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
            };
        }
    }
}
