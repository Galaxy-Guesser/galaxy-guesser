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

    //public async Task<Player> GetPlayerByIdAsync(int player_id)
    //{
    //  return await _PlayerRepository.GetPlayerByIdAsync(player_id);
    //}

    //public async Task<Player?> GetPlayerByGuidAsync(string guid)
    //{
    //  return await _PlayerRepository.GetPlayerByGuidAsync(guid);
    //}

    //public async Task<Player> CreatePlayerAsync(string guid, string username)
    //{
    //  return await _PlayerRepository.CreatePlayerAsync(guid, username);
    //}

    //public async Task<bool> UpdatePlayerAsync(int player_id, string username)
    //{
    //  var existingPlayer = await _PlayerRepository.GetPlayerByIdAsync(player_id);
    //  if (existingPlayer == null)
    //  {
    //    return false; // Player not found
    //  }

    //  return await _PlayerRepository.UpdatePlayerAsync(player_id, username);
    //}

    //public async Task<bool> DeletePlayerAsync(int player_id)
    //{
    //  var existingPlayer = await _PlayerRepository.GetPlayerByIdAsync(player_id);
    //  if (existingPlayer == null)
    //  {
    //    return false; // Player not found
    //  }

    //  return await _PlayerRepository.DeletePlayerAsync(player_id);
    //}
  }
}