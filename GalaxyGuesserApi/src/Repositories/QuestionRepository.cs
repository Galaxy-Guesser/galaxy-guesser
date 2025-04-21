using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Repositories
{
  public class QuestionRepository : IQuestionRepository
  {
    private readonly DatabaseContext _dbContext;

    public QuestionRepository(DatabaseContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<int> GetQuestionCountForCategory(int categoryId)
    {
      const string query = @"
        SELECT COUNT(q.question_id)
        FROM Categories c
        LEFT JOIN Questions q ON q.category_id = c.category_id
        WHERE c.category_id = @CategoryId
        GROUP BY c.category_id;";

      var parameters = new Dictionary<string, object> { { "@CategoryId", categoryId } };

      var result = await _dbContext.QueryAsync(query, reader => reader.GetInt32(0), parameters);

      return result.FirstOrDefault();
    }
  }
}