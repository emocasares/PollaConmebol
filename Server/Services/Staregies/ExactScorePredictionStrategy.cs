using PollaEngendrilClientHosted.Shared;

namespace PollaEngendrilClientHosted.Server.Services.Staregies
{
    public class ExactScorePredictionStrategy : IPredictionStrategy
    {
        public int CalculatePoints(MatchResult actualResult, MatchResult predictedResult)
        {
            if (actualResult.HomeTeamScore == predictedResult.HomeTeamScore &&
                actualResult.AwayTeamScore == predictedResult.AwayTeamScore)
            {
                return 5; // Exact score prediction awards 5 points.
            }
            return 0;
        }
    }
}
