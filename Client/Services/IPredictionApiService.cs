using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Client.Services
{
    public interface IPredictionApiService
    {
        Task<List<PlayerLeaderboardViewModel>> GetLeaderboardAsync();
        Task<bool> SavePredictionsAsync(PredictionRequestDTO prediction);
    }
}
