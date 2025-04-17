using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services.Interfaces
{
    public interface ISessionViewService
    {
        Task<List<SessionView>> GetAllActiveSessions();
    }
}