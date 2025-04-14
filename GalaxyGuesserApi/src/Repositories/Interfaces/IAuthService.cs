using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.src.Repositories.Interfaces
{
    public interface IAuthService
    {
        IActionResult Login();
        Task<IActionResult> Callback(string code);
    }
}