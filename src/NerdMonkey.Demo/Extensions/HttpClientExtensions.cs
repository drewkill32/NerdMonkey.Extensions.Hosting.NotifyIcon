using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using NerdMonkey.Model;

namespace NerdMonkey.Demo.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TValue> GetJsonAsync<TValue>(this HttpClient client, string requestUri)
        {
            var result = await client.GetStringAsync(requestUri);
            return JsonSerializer.Deserialize<TValue>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
