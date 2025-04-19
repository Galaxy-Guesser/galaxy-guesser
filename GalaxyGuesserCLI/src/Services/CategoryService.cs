using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using GalaxyGuesserCLI.Models;
using GalaxyGuesserCLI.Helpers;

namespace GalaxyGuesserCLI.Services
{
    public class CategoryService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<List<Categories>> GetCategoriesAsync()
        {
            try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var url = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/categories";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<Categories>>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return categories;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error retrieving categories: {ex.Message}");
                Console.ResetColor();
                return new List<Categories>();
            }
        }
    }
}