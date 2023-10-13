using Microsoft.EntityFrameworkCore;
using PollaEngendrilClientHosted.Server.Data;
using PollaEngendrilClientHosted.Server.Services.ScoringStaregies;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.Entity;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Linq;

namespace PollaEngendrilClientHosted.Server.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPredictionStrategy exactScorePredictionStrategy;
        private readonly IPredictionStrategy winnerOrTiePredictionStrategy;

        public PredictionService(IPredictionStrategy exactScorePredictionStrategy, IPredictionStrategy winnerOrTiePredictionStrategy, ApplicationDbContext dbContext)
        {
            this.exactScorePredictionStrategy = exactScorePredictionStrategy;
            this.winnerOrTiePredictionStrategy = winnerOrTiePredictionStrategy;
            this.dbContext = dbContext;
        }

        public List<PlayerLeaderboardViewModel> CalculateLeaderboard()
        {
            var actualMatches = dbContext.Matches.Where(m => m.HomeTeamScore.HasValue && m.AwayTeamScore.HasValue).ToList();
            var predictions = dbContext.Predictions
                .Where(p => dbContext.Matches
                    .Any(m => m.Id == p.MatchId && m.HomeTeamScore.HasValue && m.AwayTeamScore.HasValue))
                .ToList();
            var users = dbContext.Users.ToList();
            var userPoints = new Dictionary<Shared.Models.Entity.User, int>();
            foreach (var prediction in predictions)
            {
                var actualMatch = actualMatches.FirstOrDefault(m => m.Id == prediction.MatchId);
                var actualResult = new MatchResult() { HomeTeamScore = actualMatch.HomeTeamScore.Value, AwayTeamScore = actualMatch.AwayTeamScore.Value };
                var predictedResult = new PredictionRequestDTO
                {
                    HomeTeamScore = prediction.HomeTeamScore,
                    AwayTeamScore = prediction.AwayTeamScore
                };

                var points = CalculatePoints(actualResult, predictedResult).Points;
                var user = users.FirstOrDefault(u => u.Id == prediction.UserId);

                if (userPoints.ContainsKey(user))
                {
                    userPoints[user] += points;
                }
                else
                {
                    userPoints[user] = points;
                }
            }

            var sortedLeaderboard = userPoints.OrderByDescending(kvp => kvp.Value)
                                              .Select((kvp, index) => new PlayerLeaderboardViewModel
                                              {
                                                    Position = index + 1,
                                                    Name = kvp.Key.Email,
                                                    Score = kvp.Value
                                              }).ToList();
            return sortedLeaderboard;
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

        public async Task SavePrediction(PredictionRequestDTO prediction)
        {
            var currentPrediction = dbContext.Predictions.FirstOrDefault(u => u.MatchId == prediction.MatchId && u.UserId == prediction.UserId);

            if (currentPrediction == null)
            {
                var newPrediction = new Prediction
                {
                    UserId = prediction.UserId,
                    MatchId = prediction.MatchId,
                    HomeTeamScore = prediction.HomeTeamScore,
                    AwayTeamScore = prediction.AwayTeamScore
                };

                dbContext.Predictions.Add(newPrediction);
            }
            else
            {
                currentPrediction.HomeTeamScore = prediction.HomeTeamScore;
                currentPrediction.AwayTeamScore = prediction.AwayTeamScore;
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
