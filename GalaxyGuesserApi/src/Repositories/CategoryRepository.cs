using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
         private readonly DatabaseContext _dbContext;

        public CategoryRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            const string sql = "SELECT * FROM Categories";

            return await _dbContext.QueryAsync(sql, reader => new Category
            {
                categoryId = reader.GetInt32(0),
                category = reader.GetString(1),
            });
        }
    }
}
