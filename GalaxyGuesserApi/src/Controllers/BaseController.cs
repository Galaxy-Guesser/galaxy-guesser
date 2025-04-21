using GalaxyGuesserApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
public abstract class BaseController : ControllerBase
{


    protected string? GetGoogleIdFromClaims()
    {
        return User.FindFirst("sub")?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
