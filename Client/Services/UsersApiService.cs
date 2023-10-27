using PollaEngendrilClientHosted.Shared.Models.Entity;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Net.Http.Json;

namespace PollaEngendrilClientHosted.Client.Services
{
    public class UsersApiService : IUsersApiService
    {
        HttpClient _httpClient;

        public UsersApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateUser(string username, string nickname)
        {
            try
            {
                var data = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("nickname", nickname)
                });
                var response = await _httpClient.PostAsJsonAsync($"api/user/{username}/{nickname}", data);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var userId = await response.Content.ReadFromJsonAsync<int>();
                    return userId;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetUserIdByUserNameOrNickname(string username, string nickname)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/userid/{username}/{nickname}");
                response.EnsureSuccessStatusCode();
                var userId = await response.Content.ReadFromJsonAsync<int>();
                return userId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
