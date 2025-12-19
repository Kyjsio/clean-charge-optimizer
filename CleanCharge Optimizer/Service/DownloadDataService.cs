using CleanCharge_Optimizer.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CleanCharge_Optimizer.Service
{
    public class DownloadDataService
    {
        private readonly HttpClient _httpClient;

        public DownloadDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<GenerationData>> DownloadDataAsync(int days)
        {
            DateTime from = DateTime.UtcNow.Date;
            Console.WriteLine($"Downloading data from {from} to {from.AddDays(days)}");
            DateTime to = from.AddDays(days);

            string fromStr = from.ToString("yyyy-MM-ddTHH:mmZ");
            string toStr = to.ToString("yyyy-MM-ddTHH:mmZ");
            string url = $"https://api.carbonintensity.org.uk/generation/{fromStr}/{toStr}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<CarbonResponse>(json, options);
            Console.WriteLine(json);
            return apiResponse.Data;
        }
    }
}