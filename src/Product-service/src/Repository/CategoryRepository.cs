using Microsoft.EntityFrameworkCore;
using Product_service.Domain;
using Product_service.Persistence;

namespace Product_service.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDbContext _dbContext;
        public CategoryRepository(CategoryDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<Category> AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> FindAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category> FindCategoryByNameAsync(string Name)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name.Equals(Name));
        }
    }
}
