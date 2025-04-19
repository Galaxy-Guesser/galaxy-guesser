using GalaxyGuesserApi.Models.DTO;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        
        public SessionService(ISessionRepository _sessionRespository)
        {
            _sessionRepository = _sessionRespository;
        }

        public async Task CreateSessionAsync(CreateSessionRequestDTO requestBody,string guid)
        {
            await _sessionRepository.CreateSessionAsync(requestBody,guid);
        }

        public async Task<ActionResult<SessionDTO>> GetSessionAsync(string session_code)
        {
            return await _sessionRepository.GetSessionByCodeAsync(session_code);
        }

        public async Task<List<SessionView>> GetAllActiveSessions()
        {
            return await _sessionRepository.GetAllActiveSessions();
        }

        public async Task<List<SessionDTO>> GetAllSessionsAsync() {
            return await _sessionRepository.GetAllSessionsAsync();
        }

        public async Task<SessionDTO> GetSessionByCodeAsync(string session_code) {
            return await _sessionRepository.GetSessionByCodeAsync(session_code);
        }

        public async Task UpdateSessionAsync(SessionDTO session)
        {
            await _sessionRepository.UpdateSessionAsync(session);
        }

        public void DeleteSessionAsync(string session_code) => _sessionRepository.DeleteSessionAsync(session_code);
        public async Task JoinSessionAsync(string sessionCode, string playerGuid)
        {
            await _sessionRepository.JoinSessionAsync(sessionCode, playerGuid);
        }
    }
}