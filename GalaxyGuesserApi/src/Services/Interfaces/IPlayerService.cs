using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int playerId);
        Task<Player> CreatePlayerAsync(string guid, string userName);
        Task<bool> UpdatePlayerAsync(int playerId, string userName);
        Task<Player> GetPlayerByGuidAsync(string guid);
        Task<bool> DeletePlayerAsync(int playerId);
    }
}