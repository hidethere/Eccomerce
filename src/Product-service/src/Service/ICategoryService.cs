
using Product_service.Dto;

namespace Product_service.Service
{
    public interface ICategoryService
    {
        public Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
        public Task<CategoryDto> FindCategoryByNameAsync(string Name);
        public Task<IEnumerable<CategoryDto>> FindAllCategoriesAsync();
    }
}
