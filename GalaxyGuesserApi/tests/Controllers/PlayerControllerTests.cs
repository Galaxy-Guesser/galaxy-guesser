using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.Controllers
{
    public class PlayerControllerTests
    {
        private readonly Mock<IPlayerService> _mockService;
        private readonly PlayersController _controller;

        public PlayerControllerTests()
        {
            _mockService = new Mock<IPlayerService>();
            _controller = new PlayersController(_mockService.Object);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "test-123"),
                new Claim(ClaimTypes.NameIdentifier, "test-123"),
                new Claim("email", "test@test.com")
            }, "test"));
            
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        private Player CreateTestPlayer(int id = 1)
        {
            return new Player 
            { 
                playerId = id,
                userName = "TestPlayer",
                guid = "test-guid"
            };
        }

        [Fact]
        public async Task GetPlayers_ReturnsPlayers()
        {
            // Arrange
            var mockPlayers = new List<Player> { CreateTestPlayer() };
            _mockService.Setup(x => x.GetAllPlayersAsync()).ReturnsAsync(mockPlayers);

            // Act
            var result = await _controller.GetPlayers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockPlayers, okResult.Value);
        }

        [Fact]
        public async Task GetPlayer_ValidId_ReturnsPlayer()
        {
            // Arrange
            var mockPlayer = CreateTestPlayer();
            _mockService.Setup(x => x.GetPlayerByIdAsync(1)).ReturnsAsync(mockPlayer);

            // Act
            var result = await _controller.GetPlayer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockPlayer, okResult.Value);
        }

        [Fact]
        public async Task GetPlayer_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(x => x.GetPlayerByIdAsync(1)).ReturnsAsync((Player)null);

            // Act
            var result = await _controller.GetPlayer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreatePlayer_ReturnsCreatedPlayer()
        {
            // Arrange
            var newPlayer = CreateTestPlayer();
            _mockService.Setup(x => x.CreatePlayerAsync("guid", "name")).ReturnsAsync(newPlayer);

            // Act
            var result = await _controller.CreatePlayer("guid", "name");

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(newPlayer, createdAtResult.Value);
        }

        [Fact]
        public async Task UpdatePlayer_ValidData_ReturnsSuccess()
        {
            // Arrange
            var player = CreateTestPlayer();
            player.userName = "newName";
            _mockService.Setup(x => x.UpdatePlayerAsync(1, "newName")).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdatePlayer(1, player);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task AuthenticateOrRegister_ExistingUser_ReturnsPlayer()
        {
            // Arrange
            var existingPlayer = CreateTestPlayer();
            _mockService.Setup(x => x.GetPlayerByGuidAsync("test-123")).ReturnsAsync(existingPlayer);

            // Act
            var result = await _controller.AuthenticateOrRegister("test");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(existingPlayer, okResult.Value);
        }

        [Fact]
        public async Task DeletePlayer_ValidId_ReturnsSuccess()
        {
            // Arrange
            _mockService.Setup(x => x.DeletePlayerAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePlayer(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}