using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IPredictionService
    {
        PredictionResponseDTO CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult);

    }
}