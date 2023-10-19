using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.Entity;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IPredictionService
    {
        PredictionResponseDTO CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult);
        List<PlayerLeaderboardViewModel> CalculateLeaderboard();
        Task SavePrediction(PredictionRequestDTO newPrediction);
        List<UserPredictionViewModel> GetOthersPredictions(int matchId, int userId);
        Task<List<UserPredictionViewModel>> GetAllPredictions();
    }
}