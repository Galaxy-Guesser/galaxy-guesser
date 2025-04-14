using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.src.Middleware;
using GalaxyGuesserApi.src.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GalaxyGuesserApi.src.Services
{
    public class AuthService: IAuthService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AuthService (IHttpClientFactory httpClientFactory, IConfiguration config, IPlayerRepository playerRepository)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _playerRepository = playerRepository;
        }

        public IActionResult Login()
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

            return new RedirectResult(authUrl);
        }
        public async Task<IActionResult> Callback(string code)
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
                return new BadRequestObjectResult($"Error retrieving token: {responseContent}");

            var tokenObj = JObject.Parse(responseContent);
            var idToken = tokenObj["id_token"]?.ToString();

            if (string.IsNullOrWhiteSpace(idToken))
                return new BadRequestObjectResult("No ID token received.");

            // Validate the token securely
            var isValid = await TokenHelper.IsValidIdTokenAsync(clientId, idToken);
            if (!isValid)
                return new BadRequestObjectResult("Invalid or expired ID token.");

            //Parse the token
            var userInfo = TokenHelper.ParseIdToken(idToken);
            var guid = userInfo["sub"]?.ToString();
            var username = userInfo["given_name"]?.ToString();

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(username))
                return new BadRequestObjectResult("Missing required claims.");

            // Check if user exists
            var player = await _playerRepository.GetUserByGoogleIdAsync(guid);

            if (player == null)
            {
                player = await _playerRepository.CreatePlayerAsync(guid, username);
            }
            else
            {
                return new OkObjectResult(new
                {
                    message = "User already exists.",
                    player.player_id,
                    player.guid,
                    player.username
                });
            }

            return new OkObjectResult(new
            {
                idToken,
                player.player_id,
                player.guid,
                player.username
            });
        }
    }   
}