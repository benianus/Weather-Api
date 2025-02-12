using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ThirdPartyApi.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Internal;

namespace ThirdPartyApi;

public class ThirdPartyApiLayer
{
    private readonly HttpClient _httpClient;
    private readonly string? baseUrl;
    private readonly IConfiguration configuration;
    private readonly string? apiKey;

    public ThirdPartyApiLayer(IConfiguration Configuration, HttpClient httpClient)
    {
        configuration = Configuration;
        baseUrl = configuration["BaseUrl"];
        apiKey = configuration["ApiKey"];
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUrl);
    }
    public async Task<Weather?> GetTodayTemperatureByLocationJson(string? location)
    {
        // build the query
        string query = $"{location}/{DateTime.Now.ToString("yyyy-MM-dd")}?unitGroup=us&include=days&key={apiKey}&contentType=json";

        var response = await _httpClient.GetAsync(query);

        try
        {
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<Weather>();

                return responseBody;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return null;
    }
}
