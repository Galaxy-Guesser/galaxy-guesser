using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        /// <summary>
        /// Gets all Sessions from the database
        /// </summary>
        /// <returns>A list of all Sessions</returns>
        Task<List<SessionDTO>> GetAllSessionsAsync();

        /// <summary>
        /// Gets a Session by their ID
        /// </summary>
        /// <param name="sessionCode">The ID of the Session to retrieve</param>
        /// <returns>The PSessionlayer if found, null otherwise</returns>
        Task<SessionDTO> GetSessionByCodeAsync(string sessionCode);

        /// <summary>
        /// Creates a new Session in the database
        /// </summary>
        /// <param name="Session">The Session to create</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task CreateSessionAsync(string category, int questionsCount,string userGuid);

        /// <summary>
        /// Updates an existing Session in the database
        /// </summary>
        /// <param name="session">The Session to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdateSessionAsync(SessionDTO session);

        /// <summary>
        /// Deletes a Session from the database
        /// </summary>
        /// <param name="sessionCode">The code of the Session to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeleteSessionAsync(string sessionCode);
              /// <summary>
        /// Adds a player to an existing session
        /// </summary>
        /// <param name="sessionCode">The session code</param>
        /// <param name="player_guid">The player's GUID</param>
        Task JoinSessionAsync(string sessionCode, string playerGuid);
      
    }
}