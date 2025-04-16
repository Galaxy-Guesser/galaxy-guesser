using GalaxyGuesserApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
  public interface IQuestionRepository
  {
    Task<QuestionResponse> GetNextQuestionForSessionAsync(int sessionId);
    Task<int> GetQuestionCountForCategory(int categoryId);
    Task<List<OptionResponse>> GetOptionsByQuestionIdAsync(int questionId);
    Task<AnswerResponse> GetCorrectAnswerAsync(int questionId);
  }
}