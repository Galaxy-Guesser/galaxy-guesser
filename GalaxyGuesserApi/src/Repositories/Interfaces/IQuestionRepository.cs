using GalaxyGuesserApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
  public interface IQuestionRepository
  {
    Task<Question> GetQuestionAsync(int questionId);
    Task<List<OptionResponseDto>> GetOptionsByQuestionIdAsync(int questionId);
    Task<List<Question>> GetAllQuestionsAsync();

  }
}