using Product_service.Domain;
using Product_service.Dto;
using Product_service.Repository;
using Product_service.Exceptions;
using System.Net;

namespace Product_service.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            Category categoryFound = await _categoryRepository.FindCategoryByNameAsync(categoryDto.Name);

            if (categoryFound != null)
            {
                throw new CategoryExistsException("Category already exists!", HttpStatusCode.Conflict);
            }

            Category categoryEntity = new Category { Name = categoryDto.Name };

            await  _categoryRepository.AddAsync(categoryEntity);

            return categoryDto;
        }

        public async Task<IEnumerable<CategoryDto>> FindAllCategoriesAsync()
        {
            IEnumerable<Category> categoriesFound = await _categoryRepository.FindAllCategoriesAsync();

            if (!categoriesFound.Any())
            {
                throw new CategoriesNotFoundException("Categories not found!", HttpStatusCode.NotFound);
            }

            var categories = categoriesFound.Select(c => new CategoryDto { Name = c.Name }).ToList();

            return categories;
        }

        public async Task<CategoryDto> FindCategoryByNameAsync(string categoryName)
        {
            Category categoryFound = await _categoryRepository.FindCategoryByNameAsync(categoryName);

            if(categoryFound == null)
            {
                throw new CategoryNotFoundException("Category not found!", HttpStatusCode.NotFound);
            }

            return new CategoryDto { Name = categoryFound.Name };
        }
    }
}
