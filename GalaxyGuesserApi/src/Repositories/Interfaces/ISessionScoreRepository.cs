using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionScoreRepository
    {
        Task<bool> UpdatePlayerScoreAsync(int sessionId, int playerId, int score);
    }
}