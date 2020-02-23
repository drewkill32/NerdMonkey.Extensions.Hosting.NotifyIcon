using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NerdMonkey.Model;
using System.Text.Json;

namespace NerdMonkey.Demo.Data
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;


        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {

            var response = await _httpClient.GetAsync("weatherforecast");
            var result = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNameCaseInsensitive = true;
            var weather =  JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(result,jsonOptions) .ToArray();
            return weather;
        }
    }
}
