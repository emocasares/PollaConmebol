using PollaEngendrilClientHosted.Shared;

namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IPredictionService
    {
        int CalculatePoints(MatchResult actualResult, MatchResult predictedResult);

    }
}