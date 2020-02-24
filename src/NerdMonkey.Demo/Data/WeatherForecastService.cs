using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NerdMonkey.Model;
using System.Text.Json;
using NerdMonkey.Demo.Extensions;

namespace NerdMonkey.Demo.Data
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;


        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<WeatherForecast>> GetForecastAsync()
        {

             return _httpClient.GetJsonAsync<IEnumerable<WeatherForecast>>("weatherforecast");

        }
    }
}
