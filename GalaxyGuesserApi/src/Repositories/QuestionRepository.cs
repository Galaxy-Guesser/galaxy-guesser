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

    public async Task<QuestionResponse> GetNextQuestionForSessionAsync(int sessionId)
    {
      const string query = @"
        SELECT q.question_id, q.question
        FROM SessionQuestions sq
        JOIN Questions q ON q.question_id = sq.question_id
        WHERE sq.session_id = @sessionId
        ORDER BY q.question_id
        LIMIT 1;";

      var parameters = new Dictionary<string, object> { { "@sessionId", sessionId } };

      var result = await _dbContext.QueryAsync(query, reader => new QuestionResponse
      {
        QuestionId = reader.GetInt32(0),
        QuestionText = reader.GetString(1)
      }, parameters);

      return result.FirstOrDefault();
    }

    public async Task<List<OptionResponse>> GetOptionsByQuestionIdAsync(int questionId)
    {
      const string query = @"
        SELECT o.answer_id, a.answer
        FROM Options o
        JOIN Answers a ON a.answer_id = o.answer_id
        WHERE o.question_id = @questionId;";

      var parameters = new Dictionary<string, object> { { "@questionId", questionId } };

      var result = await _dbContext.QueryAsync(query, reader => new OptionResponse
      {
        AnswerId = reader.GetInt32(0),
        Text = reader.GetString(1)
      }, parameters);

      return result.ToList();
    }

    public async Task<AnswerResponse> GetCorrectAnswerAsync(int questionId)
    {
      const string query = @"
        SELECT a.answer_id, a.answer
        FROM Questions q
        JOIN Answers a ON a.answer_id = q.answer_id
        WHERE q.question_id = @questionId;";

      var parameters = new Dictionary<string, object> { { "@questionId", questionId } };

      var result = await _dbContext.QueryAsync(query, reader => new AnswerResponse
      {
        AnswerId = reader.GetInt32(0),
        Text = reader.GetString(1)
      }, parameters);

      return result.FirstOrDefault();
    }
  }
}