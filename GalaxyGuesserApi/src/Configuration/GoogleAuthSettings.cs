
namespace GalaxyGuesserApi.Configuration
{
    public class GoogleAuthSettings
    {
        public string clientId { get; set; } = string.Empty;
        public string clientSecret { get; set; } = string.Empty;
        public string authority { get; set; } = "https://accounts.google.com";
        public string redirectUri { get; set; } = string.Empty;
    }
}