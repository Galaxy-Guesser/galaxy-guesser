using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Services
{
  public class QuestionService
  {
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
      _questionRepository = questionRepository;
    }

    public async Task<Question> GetQuestionAsync(int questionId)
    {
      return await _questionRepository.GetQuestionAsync(questionId);
    }

    //public async Task<List<Question>> GetQuestionsBySessionIdAsync(int  sessionId)
    //{
    //  return await _questionRepository.GetQuestionsBySessionIdAsync(questionId);
    //}

    public async Task<List<OptionResponseDto>> GetOptionsByQuestionIdAsync(int questionId)
    {
      return await _questionRepository.GetOptionsByQuestionIdAsync(questionId);
    }

    public async Task<List<Question>> GetQuestionsAsync()
    {
      return await _questionRepository.GetAllQuestionsAsync();
    }
  }
}