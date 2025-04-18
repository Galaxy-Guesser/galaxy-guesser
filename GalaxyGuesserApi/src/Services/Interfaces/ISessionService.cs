using GalaxyGuesserApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Services.Interfaces
{
    public interface ISessionService
    {
        Task CreateSessionAsync(CreateSessionRequestDTO requestBody, string guid);
        Task<ActionResult<SessionDTO>> GetSessionAsync(string session_code);
        Task<List<SessionDTO>> GetAllSessionsAsync();
        Task<SessionDTO> GetSessionByCodeAsync(string session_code);
        Task UpdateSessionAsync(SessionDTO session);
        void DeleteSessionAsync(string session_code);
        Task JoinSessionAsync(string sessionCode, string playerGuid);
    }
}