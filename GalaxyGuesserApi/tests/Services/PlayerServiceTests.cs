using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.Tests{
public class PlayerServiceTests
{
    private readonly Mock<IPlayerService> _mockPlayerService;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerService = new Mock<IPlayerService>();
            _playerService = new PlayerService(_mockPlayerService.Object);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ReturnsListOfPlayers()
        {
            // Arrange
            var expectedPlayers = new List<Player>
            {
                new Player { playerId = 1, userName = "Player1", guid = "1234568" },
                new Player { playerId = 2, userName = "Player2" , guid = "1223654"}
            };

            _mockPlayerService.Setup(x => x.GetAllPlayersAsync())
                .ReturnsAsync(expectedPlayers);

            // Act
            var result = await _playerService.GetAllPlayersAsync();

            // Assert
            Assert.Equal(expectedPlayers, result);
            _mockPlayerService.Verify(x => x.GetAllPlayersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ReturnsPlayer()
        {
            // Arrange
            var expectedPlayer = new Player { playerId = 1, userName = "TestPlayer", guid = "198512" };
            _mockPlayerService.Setup(x => x.GetPlayerByIdAsync(1))
                .ReturnsAsync(expectedPlayer);

            // Act
            var result = await _playerService.GetPlayerByIdAsync(1);

            // Assert
            Assert.Equal(expectedPlayer, result);
            _mockPlayerService.Verify(x => x.GetPlayerByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPlayerByGuidAsync_ReturnsPlayer()
        {
            // Arrange
            var testGuid = "test-guid-123";
            var testUserName = "test-player";
            var expectedPlayer = new Player { playerId = 1, userName = testUserName , guid = testGuid };
            _mockPlayerService.Setup(x => x.GetPlayerByGuidAsync(testGuid))
                .ReturnsAsync(expectedPlayer);

            // Act
            var result = await _playerService.GetPlayerByGuidAsync(testGuid);

            // Assert
            Assert.Equal(expectedPlayer, result);
            _mockPlayerService.Verify(x => x.GetPlayerByGuidAsync(testGuid), Times.Once);
        }

        [Fact]
        public async Task GetPlayerByGuidAsync_ReturnsNullWhenNotFound()
        {
            // Arrange
            var testGuid = "non-existent-guid";
            _mockPlayerService.Setup(x => x.GetPlayerByGuidAsync(testGuid))
                .ReturnsAsync((Player)null);

            // Act
            var result = await _playerService.GetPlayerByGuidAsync(testGuid);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreatePlayerAsync_ReturnsNewPlayer()
        {
            // Arrange
            var testGuid = "new-guid";
            var testUserName = "NewPlayer";
            var expectedPlayer = new Player { guid = testGuid, userName = testUserName };

            _mockPlayerService.Setup(x => x.CreatePlayerAsync(testGuid, testUserName))
                .ReturnsAsync(expectedPlayer);

            // Act
            var result = await _playerService.CreatePlayerAsync(testGuid, testUserName);

            // Assert
            Assert.Equal(expectedPlayer, result);
            _mockPlayerService.Verify(x => x.CreatePlayerAsync(testGuid, testUserName), Times.Once);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ReturnsTrueWhenPlayerExists()
        {
            // Arrange
            var playerId = 1;
            var newUserName = "UpdatedName";
            var existingPlayer = new Player { playerId = playerId, guid = "12156", userName = "OldName" };

            _mockPlayerService.Setup(x => x.GetPlayerByIdAsync(playerId))
                .ReturnsAsync(existingPlayer);
            _mockPlayerService.Setup(x => x.UpdatePlayerAsync(playerId, newUserName))
                .ReturnsAsync(true);

            // Act
            var result = await _playerService.UpdatePlayerAsync(playerId, newUserName);

            // Assert
            Assert.True(result);
            _mockPlayerService.Verify(x => x.GetPlayerByIdAsync(playerId), Times.Once);
            _mockPlayerService.Verify(x => x.UpdatePlayerAsync(playerId, newUserName), Times.Once);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ReturnsFalseWhenPlayerNotFound()
        {
            // Arrange
            var playerId = 999;
            _mockPlayerService.Setup(x => x.GetPlayerByIdAsync(playerId))
                .ReturnsAsync((Player)null);

            // Act
            var result = await _playerService.UpdatePlayerAsync(playerId, "NewName");

            // Assert
            Assert.False(result);
            _mockPlayerService.Verify(x => x.GetPlayerByIdAsync(playerId), Times.Once);
            _mockPlayerService.Verify(x => x.UpdatePlayerAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeletePlayerAsync_ReturnsTrueWhenPlayerExists()
        {
            // Arrange
            var playerId = 1;
            var userName = "player";
            var guid = "112355";
            var existingPlayer = new Player { playerId = playerId, userName = userName, guid = guid };

            _mockPlayerService.Setup(x => x.GetPlayerByIdAsync(playerId))
                .ReturnsAsync(existingPlayer);
            _mockPlayerService.Setup(x => x.DeletePlayerAsync(playerId))
                .ReturnsAsync(true);

            // Act
            var result = await _playerService.DeletePlayerAsync(playerId);

            // Assert
            Assert.True(result);
            _mockPlayerService.Verify(x => x.GetPlayerByIdAsync(playerId), Times.Once);
            _mockPlayerService.Verify(x => x.DeletePlayerAsync(playerId), Times.Once);
        }

        [Fact]
        public async Task DeletePlayerAsync_ReturnsFalseWhenPlayerNotFound()
        {
            // Arrange
            var playerId = 999;
            _mockPlayerService.Setup(x => x.GetPlayerByIdAsync(playerId))
                .ReturnsAsync((Player)null);

            // Act
            var result = await _playerService.DeletePlayerAsync(playerId);

            // Assert
            Assert.False(result);
            _mockPlayerService.Verify(x => x.GetPlayerByIdAsync(playerId), Times.Once);
            _mockPlayerService.Verify(x => x.DeletePlayerAsync(It.IsAny<int>()), Times.Never);
        }
    
    }

}