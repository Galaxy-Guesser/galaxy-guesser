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
        /// <param name="session_code">The ID of the Session to retrieve</param>
        /// <returns>The PSessionlayer if found, null otherwise</returns>
        Task<SessionDTO> GetSessionByCodeAsync(string session_code);

        /// <summary>
        /// Creates a new Session in the database
        /// </summary>
        /// <param name="Session">The Session to create</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task CreateSessionAsync(string category, int questionsCount,string user_guid);

        /// <summary>
        /// Updates an existing Session in the database
        /// </summary>
        /// <param name="Session">The Session to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdateSessionAsync(SessionDTO Session);

        /// <summary>
        /// Deletes a Session from the database
        /// </summary>
        /// <param name="session_code">The code of the Session to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeleteSessionAsync(string session_code);
      
    }
}