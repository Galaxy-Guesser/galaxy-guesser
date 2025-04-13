using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.Tests{
public class PlayerServiceTests
{
    [Fact]
    public async Task GetPlayerById_WhenPlayerExists_ReturnsPlayer()
    {
        // Arrange
        var mockRepo = new Mock<IPlayerRepository>();
        var expectedPlayer = new Player { player_id = 1, username = "Test Player", guid = "123456789101112"};
        
        mockRepo.Setup(repo => repo.GetPlayerByIdAsync(1))
                .ReturnsAsync(expectedPlayer);
        
        var PlayerService = new PlayerService(mockRepo.Object);
        
        // Act
        var result = await PlayerService.GetPlayerByIdAsync(1);
        
        // Assert
        Assert.Equal(expectedPlayer.player_id, result.player_id);
        Assert.Equal(expectedPlayer.username, result.username);
        Assert.Equal(expectedPlayer.guid, result.guid);
    }
    
    [Fact]
    public async Task GetPlayerById_WhenPlayerDoesNotExist_ReturnsNull()
    {
        // Arrange
        var mockRepo = new Mock<IPlayerRepository>();
        mockRepo.Setup(repo => repo.GetPlayerByIdAsync(999))
                .ReturnsAsync((Player Player) => Player);
        
        var PlayerService = new PlayerService(mockRepo.Object);
        
        // Act
        var result = await PlayerService.GetPlayerByIdAsync(999);
        
        // Assert
        Assert.Null(result);
    }
    
}

}