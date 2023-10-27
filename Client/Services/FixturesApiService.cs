using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Net.Http.Json;

namespace PollaEngendrilClientHosted.Client.Services;
public class FixturesApiService : IFixturesApiService
{
    HttpClient _httpClient;

    public FixturesApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<FixtureViewModel>> GetFixtures(string user, string nickname)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/fixture/matches/{user}/{nickname}");
            response.EnsureSuccessStatusCode();
            var fixtures = await response.Content.ReadFromJsonAsync<List<FixtureViewModel>>();
            return fixtures;
        }
        catch
        {
            return null;
        }
    }
}
