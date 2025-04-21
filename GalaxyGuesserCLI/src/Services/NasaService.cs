using GalaxyGuesserCLI.DTO;

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

        public async Task<NasaApodResponse> GetSpaceFactAsync()
        {
           
                var response = await _httpClient.GetFromJsonAsync<NasaApodResponse>($"{ApodUrl}?api_key={ApiKey}");
                return response;
           
        }
    }
}