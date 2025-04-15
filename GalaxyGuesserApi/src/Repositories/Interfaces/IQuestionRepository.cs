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
    //Task<List<Player>> GetAllPlayersAsync();
    //Task<Player> GetPlayerByIdAsync(int playerId);
    //Task<Player> GetUserByGoogleIdAsync(string guid);
    //Task<Player> CreatePlayerAsync(string guid, string userName);
    //Task<bool> UpdatePlayerAsync(int playerId, string userName);
    //Task<bool> DeletePlayerAsync(int playerId);

    //Task<Player> GetPlayerByGuidAsync(string guid);

  }
}