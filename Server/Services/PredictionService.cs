using PollaEngendrilClientHosted.Shared;

namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IPredictionStrategy
    {
        int CalculatePoints(MatchResult actualResult, MatchResult predictedResult);
    }

    public class PredictionService : IPredictionService
    {
        private readonly IPredictionStrategy exactScorePredictionStrategy;
        private readonly IPredictionStrategy winnerOrTiePredictionStrategy;

        public PredictionService(IPredictionStrategy exactScorePredictionStrategy, IPredictionStrategy winnerOrTiePredictionStrategy)
        {
            this.exactScorePredictionStrategy = exactScorePredictionStrategy;
            this.winnerOrTiePredictionStrategy = winnerOrTiePredictionStrategy;
        }

        public int CalculatePoints(MatchResult actualResult, MatchResult predictedResult)
        {
            int points = 0;

            if (actualResult == null || predictedResult == null)
            {
                throw new ArgumentNullException("Match results cannot be null.");
            }

            if (actualResult.HomeTeamScore < 0 || actualResult.AwayTeamScore < 0 ||
                predictedResult.HomeTeamScore < 0 || predictedResult.AwayTeamScore < 0)
            {
                throw new ArgumentException("Scores cannot be negative.");
            }

            points += exactScorePredictionStrategy.CalculatePoints(actualResult, predictedResult);
            points += winnerOrTiePredictionStrategy.CalculatePoints(actualResult, predictedResult);

            return points;
        }
    }
}
