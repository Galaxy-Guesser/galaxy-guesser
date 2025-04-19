using GalaxyGuesserApi.Models.DTO;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<List<SessionDTO>> GetAllSessionsAsync();
        Task<SessionDTO> GetSessionByCodeAsync(string sessionCode);
        Task CreateSessionAsync(CreateSessionRequestDTO requestBody,string guid);
        Task UpdateSessionAsync(SessionDTO session);
        Task DeleteSessionAsync(string sessionCode);
        Task JoinSessionAsync(string sessionCode, string playerGuid);
        Task<List<SessionView>> GetAllActiveSessions();
      
    }
}