using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Net.Http.Json;

namespace PollaEngendrilClientHosted.Client.Services
{
    public class PredictionApiService : HttpClient, IPredictionApiService
    {
        HttpClient _httpClient;

        public PredictionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PlayerLeaderboardViewModel>> GetLeaderboardAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/predictions/leaderboard");
                response.EnsureSuccessStatusCode();
                var leaderboard = await response.Content.ReadFromJsonAsync<List<PlayerLeaderboardViewModel>>();
                return leaderboard;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SavePredictionsAsync(PredictionRequestDTO prediction)
        {
            var response = await _httpClient.PostAsJsonAsync("api/predictions/save-predictions", prediction);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<int> CalculatePointsAsync(PredictionRequestDTO predictionRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/predictions/calculate-points", predictionRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PredictionResponseDTO>();
                return result.Points;
            }

            return 0;
        }

        public async Task<List<OtherUserPredictionViewModel>> GetOthersPredictionsAsync(int matchId, int userId)
        {
            List<OtherUserPredictionViewModel> resultsFromOtherPlayers = new();

            return resultsFromOtherPlayers;
        }
    }
}
