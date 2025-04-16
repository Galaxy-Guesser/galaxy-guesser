using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionViewRepository
    {
        Task<List<SessionView>> GetAllActiveSessions();
    }
}
