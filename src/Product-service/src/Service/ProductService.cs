using System.Net;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Product_service.Domain;
using Product_service.Dto;
using Product_service.Event;
using Product_service.Exceptions;
using Product_service.Helper;
using Product_service.Repository;

namespace Product_service.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductMapper _productMapper;
        //private readonly BlobContainerClient _containerClient;

        public ProductService(IProductRepository productRepository, IProductMapper productMapper, ICategoryRepository categoryRepository, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _productMapper = productMapper;
            _categoryRepository = categoryRepository;
            //var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            //var containerName = configuration["AzureBlobStorage:ContainerName"];
            //_containerClient = new BlobContainerClient(connectionString, containerName);
        }
        public async Task<ProductDto> CreateProductAsync(ProductDto productdto)
        {
            Product productFound = await _productRepository.FindProductByNameAsync(productdto.Name);
            Category categoryFound = await _categoryRepository.FindCategoryByNameAsync(productdto.Category.Name);

            if(productFound != null)
            {
                throw new ProductExistsException("Product Already exists!", HttpStatusCode.Conflict);
            }

            if(categoryFound == null)
            {
                throw new CategoryNotFoundException("Category Doesnt exists!", HttpStatusCode.NotFound);
            }
            productdto.Category.Name = categoryFound.Name;

            Product product = _productMapper.DtoToEntity(productdto);
            product.CategoryId = categoryFound.Id;

            if(productdto.Image != null && productdto.Image.Length > 0)
            {
                var fileName = $"product/{product.Name}/{productdto.Image.FileName}";
               // await _containerClient.UploadBlobAsync(fileName, productdto.Image.OpenReadStream());
                //var imageUrl = _containerClient.GetBlobClient(fileName);
                //product.ImageUrl = imageUrl.Uri.ToString();
            }
            
            await _productRepository.AddAsync(product);

            return productdto;


            
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? limit)
        {
            var productQuery = _productRepository.GetProductsAsync();

            if (!productQuery.Any())
            {
                throw new ProductsNotFoundException("Products not found!", HttpStatusCode.NotFound);
            }

            if(limit.HasValue && limit.Value > 0)
            {
                productQuery = productQuery.Take(limit.Value);
            }

            var products = await productQuery.Include(p => p.Category).Select(p => _productMapper.EntityToDto(p)).ToListAsync();

            return products;
        }
        public async Task<ProductDto> SellProductAsync(ProductSoldEvent request)
        {
            var product = _productRepository.FindProductByIdAsync(request.ProductId);
            var productSoldEvent = new ProductSoldEvent
            {
                ProductId = request.ProductId,
                QuantitySold = request.QuantitySold,
                SoldAt = DateTime.Now,
            };
            return _productMapper.EntityToDto(product.Result);
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
           Product productFound = await _productRepository.FindProductByIdAsync(productId);

            if(productFound == null)
            {
                throw new ProductNotFound("Product not found!", HttpStatusCode.NotFound);
            }
            ProductDto productDto= _productMapper.EntityToDto(productFound);

            return productDto;
        }

    }
}
