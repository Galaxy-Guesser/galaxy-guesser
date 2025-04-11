using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionViewRepository
    {
        /// <summary>
        /// Gets all currently active sessions from the database view.
        /// </summary>
        /// <returns>A list of active sessions</returns>
        Task<List<SessionView>> GetAllActiveSessions();
    }
}
