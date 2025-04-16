using GalaxyGuesserCLI.Models;

namespace GalaxyGuesserCLI.Services
{
    public class NasaService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "7fxwZa5QbwcCLFZIIuCViqo2Md7eKGkv5ofpzcPD";
        private const string ApodUrl = "https://api.nasa.gov/planetary/apod";

        public NasaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSpaceFactAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<NasaApodResponse>($"{ApodUrl}?api_key={ApiKey}");
                return response.explanation;
            }
            catch
            {
                var fallbackFacts = new[]
                {
                    "The Milky Way galaxy is about 100,000 light-years across.",
                    "A neutron star can rotate up to 600 times per second.",
                    "There are more stars in space than grains of sand on Earth.",
                    "The Sun makes up 99.86% of our solar system's mass."
                };
                return fallbackFacts[new Random().Next(fallbackFacts.Length)];
            }
        }
    }
}