using Microsoft.AspNetCore.Mvc;
using Product_service.Dto;
using Product_service.Event;
using Product_service.Exceptions;
using Product_service.Service;

namespace Product_service.Controllers
{
    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ProductEventProducer _productSoldEventProducer;
        public ProductController(IProductService productService, ProductEventProducer productSoldEventProducer) 
        {
            _productService = productService;
            _productSoldEventProducer = productSoldEventProducer;
        
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto product)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);

            }
            try
            {

                ProductDto result = await _productService.CreateProductAsync(product);

                if (result == null)
                {
                    return Conflict(result);
                }

                return Ok(result);

            }
            catch (ProductServiceException ex) 
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
            catch (CategoryServiceException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });

            }
        }

        [HttpPost("sell")]
        public async Task<IActionResult> SellProduct([FromBody] ProductSoldEvent productSoldEvent)
        {

            var productSold = _productService.SellProductAsync(productSoldEvent);    


            return Ok(new {productSold, message="Product sold event published"});
        }


        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                ProductDto result = await _productService.GetProductByIdAsync(productId);

                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (ProductServiceException ex)
            {
                return StatusCode((int)ex.StatusCode, new {message = ex.Message});
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int? limit)
        {
            try
            {
                IEnumerable<ProductDto> result = await _productService.GetProductsAsync(limit);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (ProductServiceException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
        }
    }
}
