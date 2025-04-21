using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services.Interfaces;

namespace GalaxyGuesserApi.Services
{
  public class QuestionService : IQuestionService
  {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
          _questionRepository = questionRepository;
        }

         public async Task<int> GetQuestionCountForCategory(int categoryId)
        {
          return await _questionRepository.GetQuestionCountForCategory(categoryId);
        }
    }
}