using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Client.Services
{
    public interface IPredictionApiService
    {
        Task<List<PlayerLeaderboardViewModel>> GetLeaderboardAsync();
    }
}
