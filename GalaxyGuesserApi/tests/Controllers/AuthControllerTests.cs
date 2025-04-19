//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net;
//using System.Threading.Tasks;
//using System.Threading;
//using GalaxyGuesserApi.Configuration;
//using GalaxyGuesserApi.Controllers;
//using GalaxyGuesserApi.Models;
//using GalaxyGuesserApi.Repositories.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Moq.Protected;
//using Xunit;
//using static GalaxyGuesserApi.Controllers.AuthController;

//namespace GalaxyGuesserApi.Tests.Controllers
//{
//  public class AuthControllerTests
//  {
//    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
//    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
//    private readonly GoogleAuthSettings _googleAuthSettings;
//    private readonly AuthController _controller;

//    public AuthControllerTests()
//    {
//      _mockPlayerRepository = new Mock<IPlayerRepository>();
//      _mockHttpClientFactory = new Mock<IHttpClientFactory>();
//      _googleAuthSettings = new GoogleAuthSettings
//      {
//        clientId = "testClientId",
//        clientSecret = "testClientSecret",
//        redirectUri = "https://localhost:5001/api/auth/callback"
//      };

//      _controller = new AuthController(_mockHttpClientFactory.Object, _googleAuthSettings, _mockPlayerRepository.Object, new HttpClient());
//    }

//    private Mock<HttpMessageHandler> SetupMockMessageHandler(HttpResponseMessage response)
//    {
//      var mockMessageHandler = new Mock<HttpMessageHandler>();
//      mockMessageHandler.Protected()
//          .Setup<Task<HttpResponseMessage>>(
//              "SendAsync",
//              ItExpr.IsAny<HttpRequestMessage>(),
//              ItExpr.IsAny<CancellationToken>()
//          )
//          .ReturnsAsync(response);
//      return mockMessageHandler;
//    }

//    private HttpClient CreateMockHttpClient(HttpResponseMessage response)
//    {
//      var mockMessageHandler = SetupMockMessageHandler(response);
//      var httpClient = new HttpClient(mockMessageHandler.Object);
//      return httpClient;
//    }

//    public class ExchangeTokenTests : AuthControllerTests
//    {
//      [Fact]
//      public async Task ExchangeToken_NullRequest_ReturnsBadRequest()
//      {
//        TokenRequest request = null;

//        var result = await _controller.ExchangeToken(request);

//        Assert.IsType<BadRequestObjectResult>(result);
//        var badRequestResult = result as BadRequestObjectResult;
//        Assert.Equal("Invalid request", badRequestResult.Value);
//      }

//      [Fact]
//      public async Task ExchangeToken_EmptyCode_ReturnsBadRequest()
//      {
//        var request = new TokenRequest { Code = "", RedirectUri = "testUri" };

//        var result = await _controller.ExchangeToken(request);

//        Assert.IsType<BadRequestObjectResult>(result);
//        var badRequestResult = result as BadRequestObjectResult;
//        Assert.Equal("Invalid request", badRequestResult.Value);
//      }

//      [Fact]
//      public async Task ExchangeToken_FailedExchange_ReturnsBadRequest()
//      {
//        var request = new TokenRequest { Code = "testCode", RedirectUri = _googleAuthSettings.redirectUri };
//        var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          Content = new StringContent("{\"error\": \"invalid_grant\"}")
//        };
//        var mockHttpClient = CreateMockHttpClient(mockResponse);
//        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

//        var result = await _controller.ExchangeToken(request);

//        Assert.IsType<BadRequestObjectResult>(result);
//        var badRequestResult = result as BadRequestObjectResult;
//        Assert.StartsWith("Failed to exchange code for token", badRequestResult.Value.ToString());
//        Assert.Contains(_googleAuthSettings.redirectUri, badRequestResult.Value.ToString());
//      }

//    }

//    public class LoginTests : AuthControllerTests
//    {
//      [Fact]
//      public void Login_ReturnsRedirectResultWithCorrectUrl()
//      {
//        var expectedAuthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
//                              $"client_id={_googleAuthSettings.clientId}" +
//                              $"&redirect_uri={_googleAuthSettings.redirectUri}" +
//                              $"&response_type=code" +
//                              $"&scope={Uri.EscapeDataString("openid email profile")}" +
//                              $"&access_type=offline" +
//                              $"&prompt=consent";

//        var result = _controller.Login();

//        Assert.IsType<RedirectResult>(result);
//        var redirectResult = result as RedirectResult;
//        Assert.Equal(expectedAuthUrl, redirectResult.Url);
//        Assert.False(redirectResult.Permanent);
//      }
//    }

//    public class CallbackTests : AuthControllerTests
//    {
//      [Fact]
//      public async Task Callback_InvalidCode_ReturnsBadRequest()
//      {
//        var code = "invalidCode";
//        var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
//        {
//          Content = new StringContent("{\"error\": \"invalid_request\"}")
//        };
//        var mockHttpClient = CreateMockHttpClient(mockResponse);
//        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

//        var result = await _controller.Callback(code);

//        Assert.IsType<BadRequestObjectResult>(result);
//        var badRequestResult = result as BadRequestObjectResult;
//        Assert.StartsWith("Error retrieving token", badRequestResult.Value.ToString());
//      }

//      [Fact]
//      public async Task Callback_NoIdToken_ReturnsBadRequest()
//      {
//        var code = "validCode";
//        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
//        {
//          Content = new StringContent("{\"access_token\": \"testToken\"}")
//        };
//        var mockHttpClient = CreateMockHttpClient(mockResponse);
//        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

//        var result = await _controller.Callback(code);

//        Assert.IsType<BadRequestObjectResult>(result);
//        var badRequestResult = result as BadRequestObjectResult;
//        Assert.Equal("No ID token received.", badRequestResult.Value);
//      }

//      [Fact]
//      public async Task Callback_MissingClaims_ReturnsBadRequest()
//      {
//        var code = "validCode";
//        var idToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: new[]
//        {
//                new System.Security.Claims.Claim("email", "test@example.com")
//            }));
//        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
//        {
//          Content = new StringContent($"{{\"id_token\": \"{idToken}\"}}")
//        };
//        var mockHttpClient = CreateMockHttpClient(mockResponse);
//        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);

//        var result = await _controller.Callback(code);

//        Assert.IsType<BadRequestObjectResult>(result);
//        var badRequestResult = result as BadRequestObjectResult;
//        Assert.Equal("Missing required claims.", badRequestResult.Value);
//      }

//      [Fact]
//      public async Task Callback_ExistingUser_ReturnsOkWithMessageAndPlayerInfo()
//      {
//        var code = "validCode";
//        var guid = "existingGuid";
//        var userName = "Existing User";
//        var playerId = 1;
//        var idToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: new[]
//        {
//                new System.Security.Claims.Claim("sub", guid),
//                new System.Security.Claims.Claim("given_name", userName)
//            }));
//        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
//        {
//          Content = new StringContent($"{{\"id_token\": \"{idToken}\"}}")
//        };
//        var mockHttpClient = CreateMockHttpClient(mockResponse);
//        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);
//        _mockPlayerRepository.Setup(repo => repo.GetUserByGoogleIdAsync(guid)).ReturnsAsync(new Player { playerId = playerId, guid = guid, userName = userName });

//        var result = await _controller.Callback(code);

//        Assert.IsType<OkObjectResult>(result);
//        var okResult = result as OkObjectResult;
//        Assert.NotNull(okResult.Value);
//        var responseData = okResult.Value as dynamic;
//        Assert.Equal("User already exists.", responseData.message);
//        Assert.Equal(playerId, responseData.playerId);
//        Assert.Equal(guid, responseData.guid);
//        Assert.Equal(userName, responseData.userName);
//        _mockPlayerRepository.Verify(repo => repo.CreatePlayerAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
//      }
//    }
//  }
//}
