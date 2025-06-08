using Microsoft.AspNetCore.Mvc;
using Product_service.Dto;
using Product_service.Exceptions;
using Product_service.Service;

namespace Product_service.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController :ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                var result = await _categoryService.CreateCategoryAsync(categoryDto);
                if(result == null)
                {
                    return Conflict(result);
                }

                return Ok(result);
            }
            catch (CategoryServiceException ex)
            {
                return StatusCode((int)ex.StatusCode, new { message = ex.Message });
            }
        }

        [HttpGet("{categoryName}")]
        public async Task<IActionResult> GetCategoryByName(string categoryName)
        {
            try
            {
                var result = await _categoryService.FindCategoryByNameAsync(categoryName);
                if(result == null)
                {
                    return Conflict(result);
                }

                return Ok(result);

            }
            catch(CategoryServiceException ex)
            {
                return StatusCode((int) ex.StatusCode, new {message = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _categoryService.FindAllCategoriesAsync();
                if(result == null)
                {
                    return Conflict(result);
                }
                return Ok(result);

            }
            catch (CategoryServiceException ex)
            {
                return StatusCode((int) ex.StatusCode, new {message = ex.Message}); 
            }
        }
    }
}
