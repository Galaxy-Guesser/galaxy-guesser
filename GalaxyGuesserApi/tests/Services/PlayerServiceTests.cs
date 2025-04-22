using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.tests.Services
{
  public class PlayerServiceTests
  {
    private readonly Mock<IPlayerRepository> _mockRepo;
    private readonly PlayerService _service;

    public PlayerServiceTests()
    {
      _mockRepo = new Mock<IPlayerRepository>();
      _service = new PlayerService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllPlayersAsync_ReturnsAllPlayers()
    {
      var players = new List<Player> { new Player() { guid = "googleguid", userName = "user1"}, new Player() { guid = "googleguid2", userName = "user2"} };
      _mockRepo.Setup(repo => repo.GetAllPlayersAsync()).ReturnsAsync(players);

      var result = await _service.GetAllPlayersAsync();

      Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_ReturnsPlayer()
    {
      var player = new Player { playerId = 1, guid = "googleguid", userName = "user1" };
      _mockRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);

      var result = await _service.GetPlayerByIdAsync(1);

      Assert.Equal(1, result.playerId);
    }

    [Fact]
    public async Task CreatePlayerAsync_ReturnsCreatedPlayer()
    {
      var player = new Player { playerId = 1, guid = "abc123", userName = "test" };
      _mockRepo.Setup(repo => repo.CreatePlayerAsync("abc123", "test")).ReturnsAsync(player);

      var result = await _service.CreatePlayerAsync("abc123", "test");

      Assert.Equal("test", result.userName);
    }

    [Fact]
    public async Task UpdatePlayerUsernameAsync_WhenPlayerExists_ReturnsTrue()
    {
      var player = new Player { playerId = 1, userName = "OldName", guid = "googleguid" };
      _mockRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
      _mockRepo.Setup(repo => repo.UpdatePlayerUsernameAsync(1, "NewName")).ReturnsAsync(true);

      var result = await _service.UpdatePlayerUsernameAsync(1, "NewName");

      Assert.True(result);
    }


    [Fact]
    public async Task GetPlayerByGuidAsync_ReturnsPlayer()
    {
      var player = new Player { guid = "abc123", userName = "user1" };
      _mockRepo.Setup(repo => repo.GetPlayerByGuidAsync("abc123")).ReturnsAsync(player);

      var result = await _service.GetPlayerByGuidAsync("abc123");

      Assert.Equal("abc123", result.guid);
    }

    [Fact]
    public async Task DeletePlayerAsync_WhenPlayerExists_ReturnsTrue()
    {
      var player = new Player { playerId = 1, userName = "user1", guid="googleguid" };
      _mockRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
      _mockRepo.Setup(repo => repo.DeletePlayerAsync(1)).ReturnsAsync(true);

      var result = await _service.DeletePlayerAsync(1);

      Assert.True(result);
    }

    [Fact]
    public async Task GetPlayersStats_ReturnsStats()
    {
      var stats = new List<PlayerStatsDTO> { new PlayerStatsDTO(), new PlayerStatsDTO() };
      _mockRepo.Setup(repo => repo.GetPlayersStats(1)).ReturnsAsync(stats);

      var result = await _service.GetPlayersStats(1);

      Assert.Equal(2, result.Count);
    }
  }
}