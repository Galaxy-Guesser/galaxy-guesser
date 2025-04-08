using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Gets all Players from the database
        /// </summary>
        /// <returns>A list of all Players</returns>
        Task<List<Player>> GetAllPlayersAsync();

        /// <summary>
        /// Gets a Player by their ID
        /// </summary>
        /// <param name="id">The ID of the Player to retrieve</param>
        /// <returns>The Player if found, null otherwise</returns>
        Task<Player> GetPlayerByIdAsync(int id);

        /// <summary>
        /// Creates a new Player in the database
        /// </summary>
        /// <param name="Player">The Player to create</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task CreatePlayerAsync(Player Player);

        /// <summary>
        /// Updates an existing Player in the database
        /// </summary>
        /// <param name="Player">The Player to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdatePlayerAsync(Player Player);

        /// <summary>
        /// Deletes a Player from the database
        /// </summary>
        /// <param name="id">The ID of the Player to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeletePlayerAsync(int id);

        /// <summary>
        /// Finds Players by email address
        /// </summary>
        /// <param name="email">The email to search for</param>
        /// <returns>Players matching the email address</returns>
        Task<Player> GetPlayerByEmailAsync(string email);

        /// <summary>
        /// Checks if a Player with the given email already exists
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>True if the Player exists, false otherwise</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}