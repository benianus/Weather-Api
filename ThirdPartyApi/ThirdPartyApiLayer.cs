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
    HttpClient _httpClient = new HttpClient();
    string baseUrl = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";
    public ThirdPartyApiLayer()
    {
        _httpClient.BaseAddress = new Uri(baseUrl);

    }
    public string QueryBuilder(string location)
    {
        var builder = new UriBuilder(baseUrl);
        var query = HttpUtility.ParseQueryString(builder.Query);
        var finalQuery = string.Empty;

        query["unitGroup"] = "us";
        query["key"] = "W38JCLTCKR6FQYVLU9TX4SY2K";
        query["contentType"] = "json";

        builder.Query = query.ToString();

        finalQuery = builder.ToString();

        return finalQuery;
    }
    public async Task<Weather?> GetTodayTemperatureByLocationJson(string? location, string? apiKey)
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
