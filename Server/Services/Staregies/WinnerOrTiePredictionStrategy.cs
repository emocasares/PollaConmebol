using PollaEngendrilClientHosted.Shared;

namespace PollaEngendrilClientHosted.Server.Services.Staregies
{
    public class WinnerOrTiePredictionStrategy : IPredictionStrategy
    {
        public int CalculatePoints(MatchResult actualResult, MatchResult predictedResult)
        {
            if ((actualResult.HomeTeamScore > actualResult.AwayTeamScore ==
                 predictedResult.HomeTeamScore > predictedResult.AwayTeamScore) ||
                (actualResult.HomeTeamScore < actualResult.AwayTeamScore ==
                 predictedResult.HomeTeamScore < predictedResult.AwayTeamScore) ||
                (actualResult.HomeTeamScore == actualResult.AwayTeamScore &&
                 predictedResult.HomeTeamScore == predictedResult.AwayTeamScore))
            {
                return 3; // Winner or tie prediction awards 3 points.
            }
            return 0;
        }
    }
}
