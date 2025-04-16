using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using GalaxyGuesserApi.Repositories.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using GalaxyGuesserApi.Configuration;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;  

        private readonly GoogleAuthSettings _googleAuth;

        public AuthController(IHttpClientFactory httpClientFactory, GoogleAuthSettings googleAuth, IPlayerRepository playerRepository , HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;  
            _googleAuth = googleAuth;
            _playerRepository = playerRepository;
        }

        [HttpPost("token")]
        public async Task<IActionResult> ExchangeToken([FromForm] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Code))
            {
                return BadRequest("Invalid request");
            }

            try 
            {
                var tokenRequestParams = new Dictionary<string, string>
                {
                    ["client_id"] =   _googleAuth.clientId,
                    ["client_secret"] =  _googleAuth.clientSecret,
                    ["code"] = request.Code,
                    ["redirect_uri"] = request.RedirectUri,
                    ["grant_type"] = "authorization_code"
                };

                var content = new FormUrlEncodedContent(tokenRequestParams);
                var response = await _httpClient.PostAsync(
                    "https://oauth2.googleapis.com/token", 
                    content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest("Failed to exchange code for token" + request.RedirectUri);
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var googleTokens = JsonSerializer.Deserialize<GoogleTokenResponse>(responseContent);

                
                return Ok(new { access_token = googleTokens.IdToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        
        public class TokenRequest
        {
            public string Code { get; set; }
            public string RedirectUri { get; set; }
        }

        public class GoogleTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
            
            [JsonPropertyName("id_token")]
            public string IdToken { get; set; }
            
            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }
            
            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }

        [HttpGet("login")]
        public IActionResult Login( )
        {
            var clientId =  _googleAuth.clientId;
            var redirectUri = _googleAuth.redirectUri;
            var scope = "openid email profile";

            var authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                        $"client_id={clientId}" +
                        $"&redirect_uri={redirectUri}" +
                        $"&response_type=code" +
                        $"&scope={Uri.EscapeDataString(scope)}" +
                        $"&access_type=offline" +
                        $"&prompt=consent";

            return Redirect(authUrl);
        }

        [AllowAnonymous]
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            var clientId =  _googleAuth.clientId;
            var clientSecret =  _googleAuth.clientSecret;
            var redirectUri = _googleAuth.redirectUri;

            var httpClient = _httpClientFactory.CreateClient();

            var requestBody = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" }
        };

            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(requestBody));
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest($"Error retrieving token: {responseContent}");

            var tokenObj = JObject.Parse(responseContent);
            var idToken = tokenObj["id_token"]?.ToString();

            if (string.IsNullOrWhiteSpace(idToken))
                return BadRequest("No ID token received.");

            // Decode ID token
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(idToken);

            var guid = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var userName = jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(userName))
                return BadRequest("Missing required claims.");

                var player = await _playerRepository.GetUserByGoogleIdAsync(guid);

                if (player == null)
                {
                    player = await _playerRepository.CreatePlayerAsync(guid, userName);
                }
                else
                {
                    return Ok(new
                    {
                        message = "User already exists.",
                        player.playerId,
                        player.guid,
                        player.userName
                    });
                }

            return Ok(new
            {
                idToken,
                player.playerId,
                player.guid,
                player.userName
            });
        }
          
    }
}