using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Services
{
    public class SessionScoreService
    {
        private readonly ISessionScoreRepository _sessionScoreRepository;

        public SessionScoreService(ISessionScoreRepository sessionScoreRepository)
        {
            _sessionScoreRepository = sessionScoreRepository;
        }
        public async Task<bool> UpdatePlayerScoreAsync(int sessionId, int playerId, int score)
        {
           return await _sessionScoreRepository.UpdatePlayerScoreAsync(sessionId, playerId, score);
        }
    }
}