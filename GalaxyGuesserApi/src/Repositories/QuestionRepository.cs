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

    public async Task<Question> GetQuestionAsync(int questionId)
    {
      const string query = @"
                            SELECT question_id, question, answer_id, category_id
                            FROM questions
                            WHERE question_id = @questionId";
      var parameters = new Dictionary<string, object> { { "@questionId", questionId } };

      var result = await _dbContext.QueryAsync(query, reader => new Question
      {
        questionId = reader.GetInt32(0),
        question = reader.GetString(1),
        answerId = reader.GetInt32(2),
        categoryId = reader.GetInt32(3)
      }, parameters);
      return result.FirstOrDefault()!;
    }

    public async Task<List<OptionResponseDto>> GetOptionsByQuestionIdAsync(int questionId)
    {
      const string query = @"
                            SELECT question_id, text
                            FROM options
                            WHERE question_id = @questionId";

      var options = new List<OptionResponseDto>();

      var parameters = new Dictionary<string, object> { { "@questionId", questionId } };

      var results = await _dbContext.QueryAsync(query, reader => new OptionResponseDto
      {
        Id = reader.GetInt32(0),
        Text = reader.GetString(1)
      }, parameters);

      return results.ToList();
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {

      const string query = @"
            SELECT q.question_id AS questionId, q.question AS question_text,
            o.options_id AS options_id, o.text AS option_text, o.isCorrect
            FROM questions q
            JOIN options o ON q.question_id = o.question_id
            ORDER BY q.question_id, o.options_id";

      var questionsMap = new Dictionary<int, Question>();

      var rows = await _dbContext.QueryAsync<QuestionOptionRow>(query, reader => new QuestionOptionRow
      {
        QuestionId = reader.GetInt32(0),
        QuestionText = reader.GetString(1),
        OptionId = reader.GetInt32(2),
        OptionText = reader.GetString(3),
        IsCorrect = reader.GetBoolean(4)
      });

      var questions = rows
        .GroupBy(r => new { r.QuestionId, r.QuestionText })
        .Select(group => new Question
        {
          questionId = group.Key.QuestionId,
          question = group.Key.QuestionText,
          Options = group.Select(r => new Option
          {
            Id = r.OptionId,
            Text = r.OptionText,
            IsCorrect = r.IsCorrect
          }).ToList()
        })
        .ToList();

      return questions;

    }

    public class QuestionOptionRow
    {
      public int QuestionId { get; set; }
      public string QuestionText { get; set; }
      public int OptionId { get; set; }
      public string OptionText { get; set; }
      public bool IsCorrect { get; set; }
    }
  }
}