using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyGuesserCLI.src.Models
{
    public class OAuthResult
    {

    public string IdToken { get; set; } = string.Empty;
    public int PlayerId { get; set; }
    public string Guid { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    
    }
}