using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

namespace PollaEngendrilClientHosted.Server.Services.ScoringStaregies
{
    public class WinnerOrTiePredictionStrategy : IPredictionStrategy
    {
        public int CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult)
        {
            bool actualHomeTeamWins = actualResult.HomeTeamScore > actualResult.AwayTeamScore;
            bool predictedHomeTeamWins = predictedResult.HomeTeamScore > predictedResult.AwayTeamScore;
            bool actualAwayTeamWins = actualResult.HomeTeamScore < actualResult.AwayTeamScore;
            bool predictedAwayTeamWins = predictedResult.HomeTeamScore < predictedResult.AwayTeamScore;
            bool actualIsTie = actualResult.HomeTeamScore == actualResult.AwayTeamScore;
            bool predictedIsTie = predictedResult.HomeTeamScore == predictedResult.AwayTeamScore;

            if ((actualHomeTeamWins == predictedHomeTeamWins) ||
                (actualAwayTeamWins == predictedAwayTeamWins) ||
                (actualIsTie && predictedIsTie))
            {
                return 3;
            }
            return 0;
        }
    }
}
