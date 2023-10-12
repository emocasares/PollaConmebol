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

        public async Task<int> CreateUser(string username)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/user/{username}", username);
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

        public async Task<int> GetUserIdByUserName(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/userid/{username}");
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
