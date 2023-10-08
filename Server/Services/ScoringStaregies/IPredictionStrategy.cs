using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

namespace PollaEngendrilClientHosted.Server.Services.ScoringStaregies
{
    public interface IPredictionStrategy
    {
        int CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult);
    }
}
