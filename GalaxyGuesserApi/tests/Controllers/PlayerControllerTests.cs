using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using GalaxyGuesserApi.Controllers;
using GalaxyGuesserApi.Services;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GalaxyGuesserApi.Tests.Controllers
{
  public class PlayersControllerTests
  {
    private readonly Mock<IPlayerService> _mockService;
    private readonly PlayersController _controller;

    public PlayersControllerTests()
    {
      _mockService = new Mock<IPlayerService>();
      _controller = new PlayersController(_mockService.Object);
    }

    private void SetUserWithGoogleId(string googleId)
    {
      var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
      {
                new Claim("sub", googleId)
      }, "mock"));

      _controller.ControllerContext = new ControllerContext()
      {
        HttpContext = new DefaultHttpContext() { User = user }
      };
    }

    [Fact]
    public async Task GetPlayers_ReturnsOk_WithListOfPlayers()
    {
      var mockPlayers = new List<Player> { new Player { playerId = 1, userName = "Player1", guid = "google-123" } };
      _mockService.Setup(service => service.GetAllPlayersAsync()).ReturnsAsync(mockPlayers);

      var result = await _controller.GetPlayers();

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var response = Assert.IsType<ApiResponse<IEnumerable<Player>>>(okResult.Value);
      Assert.True(response.Success);
      Assert.Single(response.Data);
    }

    [Fact]
    public async Task GetPlayer_ReturnsNotFound_WhenPlayerDoesNotExist()
    {
      SetUserWithGoogleId("google-123");
      _mockService.Setup(s => s.GetPlayerByGuidAsync("google-123")).ReturnsAsync((Player?)null);

      var result = await _controller.GetPlayer(1);

      var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
      var response = Assert.IsType<ApiResponse<Player>>(notFound.Value);
      Assert.False(response.Success);
    }

    [Fact]
    public async Task CreatePlayer_ReturnsCreated_WhenSuccessful()
    {
      SetUserWithGoogleId("google-123");
      var player = new Player { playerId = 5, userName = "newuser", guid = "google-123" };
      _mockService.Setup(s => s.CreatePlayerAsync("google-123", "newuser")).ReturnsAsync(player);

      var request = new CreatePlayerRequest { UserName = "newuser" };

      var result = await _controller.CreatePlayer(request);

      var created = Assert.IsType<CreatedAtActionResult>(result.Result);
      var response = Assert.IsType<ApiResponse<Player>>(created.Value);
      Assert.True(response.Success);
      Assert.Equal("newuser", response.Data.userName);
    }

    [Fact]
    public async Task GetPlayersStats_ReturnsOk_WhenStatsExist()
    {
      SetUserWithGoogleId("google-123");
      var stats = new List<PlayerStatsDTO>
            {
                new PlayerStatsDTO { sessionCode = "ABC123", category = "Space", highestScore = 90 }
            };
      _mockService.Setup(s => s.GetPlayersStats(1)).ReturnsAsync(stats);

      var result = await _controller.GetPlayersStats(1);

      var ok = Assert.IsType<OkObjectResult>(result.Result);
      var response = Assert.IsType<ApiResponse<IEnumerable<PlayerStatsDTO>>>(ok.Value);
      Assert.True(response.Success);
      Assert.Single(response.Data);
    }

    [Fact]
    public async Task UpdatePlayerUsername_ReturnsBadRequest_WhenUsernameIsEmpty()
    {
      SetUserWithGoogleId("google-123");
      var request = new UpdatePlayerRequest { UserName = "" };

      var result = await _controller.UpdatePlayerUsernameAsync(1, request);

      var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
      var response = Assert.IsType<ApiResponse<Player>>(badRequest.Value);
      Assert.False(response.Success);
    }

    [Fact]
    public async Task DeletePlayer_ReturnsOk_WhenDeletedSuccessfully()
    {
      SetUserWithGoogleId("google-123");
      var player = new Player { playerId = 1, userName = "user1", guid = "google-123" };
      _mockService.Setup(s => s.GetPlayerByGuidAsync("google-123")).ReturnsAsync(player);
      _mockService.Setup(s => s.DeletePlayerAsync(1)).ReturnsAsync(true);

      var result = await _controller.DeletePlayer(1);

      var ok = Assert.IsType<OkObjectResult>(result.Result);
      var response = Assert.IsType<ApiResponse<Player?>>(ok.Value);
      Assert.True(response.Success);
    }

    [Fact]
    public async Task AuthenticateOrRegister_ReturnsNewPlayer_WhenNotFound()
    {
      SetUserWithGoogleId("google-999");
      _mockService.Setup(s => s.GetPlayerByGuidAsync("google-999")).ReturnsAsync((Player?)null);
      _mockService.Setup(s => s.CreatePlayerAsync("google-999", " "))
                  .ReturnsAsync(new Player { playerId = 100, userName = "user1",  guid = "google-999" });

      var result = await _controller.AuthenticateOrRegister("SomeName");

      var ok = Assert.IsType<OkObjectResult>(result.Result);
      var response = Assert.IsType<ApiResponse<Player?>>(ok.Value);
      Assert.True(response.Success);
      Assert.Equal("Player registered successfully", response.Message);
    }
  }
}