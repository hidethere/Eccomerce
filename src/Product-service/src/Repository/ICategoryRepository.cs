using Product_service.Domain;

namespace Product_service.Repository
{
    public interface ICategoryRepository
    {
        public Task<Category> AddAsync(Category category);
        public Task<Category> FindCategoryByNameAsync(string Name);
        public Task<IEnumerable<Category>> FindAllCategoriesAsync();
    }
}
