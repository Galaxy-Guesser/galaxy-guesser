using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Repositories
{
    public class SessionService
    {
        private readonly ISessionRepository _sessionRepository;
        
        public SessionService(ISessionRepository _sessionRespository)
        {
            _sessionRepository = _sessionRespository;
        }

        public async Task CreateSessionAsync(string category, int questionsCount,string user_guid)
        {
            await _sessionRepository.CreateSessionAsync(category, questionsCount, user_guid);
        }

        public async Task<ActionResult<SessionDTO>> GetSessionAsync(string session_code)
        {
            return await _sessionRepository.GetSessionByCodeAsync(session_code);
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
    }
}