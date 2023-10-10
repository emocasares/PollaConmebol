using PollaEngendrilClientHosted.Server.Services.ScoringStaregies;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Server.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IPredictionStrategy exactScorePredictionStrategy;
        private readonly IPredictionStrategy winnerOrTiePredictionStrategy;

        public PredictionService(IPredictionStrategy exactScorePredictionStrategy, IPredictionStrategy winnerOrTiePredictionStrategy)
        {
            this.exactScorePredictionStrategy = exactScorePredictionStrategy;
            this.winnerOrTiePredictionStrategy = winnerOrTiePredictionStrategy;
        }

        public List<PlayerLeaderboardViewModel> CalculateLeaderboard()
        {

            var leaderboard = new List<PlayerLeaderboardViewModel>
            {
                new PlayerLeaderboardViewModel { Position = 1, Name = "Jugador 1", Score = 20 },
                new PlayerLeaderboardViewModel { Position = 2, Name = "Jugador 2", Score = 18 },
            };

            return leaderboard;
        }

        public PredictionResponseDTO CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult)
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

            var response = new PredictionResponseDTO
            {
                Points = points
            };

            return response;
        }
    }
}
