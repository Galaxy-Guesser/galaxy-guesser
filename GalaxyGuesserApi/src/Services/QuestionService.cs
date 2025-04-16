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

    public async Task<QuestionResponse> GetNextQuestionForSessionAsync(int sessionId)
    {
      return await _questionRepository.GetNextQuestionForSessionAsync(sessionId);
    }

    public async Task<List<OptionResponse>> GetOptionsByQuestionIdAsync(int questionId)
    {
      return await _questionRepository.GetOptionsByQuestionIdAsync(questionId);
    }

    public async Task<AnswerResponse> GetCorrectAnswerAsync(int questionId)
    {
      return await _questionRepository.GetCorrectAnswerAsync(questionId);
    }
  }
}