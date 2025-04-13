using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration config, IPlayerRepository playerRepository)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _playerRepository = playerRepository;
        }

        [HttpGet("login")]
        public IActionResult Login( )
        {
            var clientId = _config["Google:ClientId"];
            var redirectUri = _config["Google:RedirectUri"];
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
            var clientId = _config["Google:ClientId"];
            var clientSecret = _config["Google:ClientSecret"];
            var redirectUri = _config["Google:RedirectUri"];

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
            var username = jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(username))
                return BadRequest("Missing required claims.");

                // Check if user exists in DB
                var player = await _playerRepository.GetUserByGoogleIdAsync(guid);

                if (player == null)
                {
                    player = await _playerRepository.CreatePlayerAsync(guid, username);
                }
                else
                {
                    return Ok(new
                    {
                        message = "User already exists.",
                        player.player_id,
                        player.guid,
                        player.username
                    });
                }

            return Ok(new
            {
                idToken,
                player.player_id,
                player.guid,
                player.username
            });
        }
          
    }
}