using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

namespace PollaEngendrilClientHosted.Server.Services.ScoringStaregies
{
    public class ExactScorePredictionStrategy : IPredictionStrategy
    {
        public int CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult)
        {
            if (actualResult.HomeTeamScore == predictedResult.HomeTeamScore &&
                actualResult.AwayTeamScore == predictedResult.AwayTeamScore)
            {
                return 5;
            }
            return 0;
        }
    }
}
