using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.src.Repositories.Interfaces;

namespace GalaxyGuesserApi.src.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;

            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }

            [HttpGet("login")]
            public IActionResult Login()
            {
                return _authService.Login();
            }

            [AllowAnonymous]
            [HttpGet("callback")]
            public async Task<IActionResult> Callback([FromQuery] string code)
            {
                try
                {
                    return await _authService.Callback(code);
                }
                catch (Exception ex)
                {                   
                    return StatusCode(
                        StatusCodes.Status500InternalServerError, 
                        $"Error handling callback: {ex.Message}"
                    );
                }
            }
    }
}