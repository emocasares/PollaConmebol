using Microsoft.EntityFrameworkCore;
using PollaEngendrilClientHosted.Server.Data;
using PollaEngendrilClientHosted.Server.Services.ScoringStaregies;
using PollaEngendrilClientHosted.Server.Services.UserEligibleSpecification;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.Entity;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Linq;
using System.Reflection;

namespace PollaEngendrilClientHosted.Server.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPredictionStrategy exactScorePredictionStrategy;
        private readonly IPredictionStrategy winnerOrTiePredictionStrategy;
        private readonly IUserEligibleSpecification userEligibleSpecification;

        public PredictionService(IEnumerable<IPredictionStrategy> predictionStrategies, IUserEligibleSpecification userSpecification, ApplicationDbContext dbContext)
        {
            this.exactScorePredictionStrategy = predictionStrategies?
                        .FirstOrDefault(s => s.GetType().GetCustomAttribute<StrategyNameAttribute>()?.Name == "ExactScore");

            this.winnerOrTiePredictionStrategy = predictionStrategies?
                .FirstOrDefault(s => s.GetType().GetCustomAttribute<StrategyNameAttribute>()?.Name == "WinnerOrTie");

            this.userEligibleSpecification = userSpecification;
            this.dbContext = dbContext;
        }

        public List<PlayerLeaderboardViewModel> CalculateLeaderboard()
        {
            var actualMatches = dbContext.Matches.Where(m => m.HomeTeamScore.HasValue && m.AwayTeamScore.HasValue).ToList();
            var predictions = dbContext.Predictions
                .Where(p => dbContext.Matches
                    .Any(m => m.Id == p.MatchId && m.HomeTeamScore.HasValue && m.AwayTeamScore.HasValue))
                .ToList();
            var users = dbContext.Users.Where(this.userEligibleSpecification.IsSatisfiedBy).ToList();
            var userPoints = new Dictionary<Shared.Models.Entity.User, int>();
            foreach (var prediction in predictions.Where(p => users.Any(u => u.Id == p.UserId)))
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

                if (user is not null)
                {
                    if (userPoints.ContainsKey(user))
                    {
                        userPoints[user] += points;
                    }
                    else
                    {
                        userPoints[user] = points;
                    }
                }
            }

            var sortedLeaderboard = userPoints.OrderByDescending(kvp => kvp.Value)
                                              .Select((kvp, index) => new PlayerLeaderboardViewModel
                                              {
                                                  Position = index + 1,
                                                  Name = kvp.Key.UserName,
                                                  Score = kvp.Value
                                              }).ToList();
            return sortedLeaderboard;
        }

        public PredictionResponseDTO CalculatePoints(MatchResult actualResult, PredictionRequestDTO predictedResult)
        {
            int points = 0;
            if (!actualResult.AwayTeamScore.HasValue || !actualResult.HomeTeamScore.HasValue)
            {
                return new PredictionResponseDTO
                {
                    Points = 0
                };
            }

            if (actualResult == null || predictedResult == null)
            {
                throw new ArgumentNullException("Match results cannot be null.");
            }

            if (actualResult.HomeTeamScore < 0 || actualResult.AwayTeamScore < 0 ||
                predictedResult.HomeTeamScore < 0 || predictedResult.AwayTeamScore < 0)
            {
                throw new ArgumentException("Scores cannot be negative.");
            }

            var pointsOriginalValue = points;
            points += exactScorePredictionStrategy.CalculatePoints(actualResult, predictedResult);

            if (points == pointsOriginalValue)
                points += winnerOrTiePredictionStrategy.CalculatePoints(actualResult, predictedResult);

            var response = new PredictionResponseDTO
            {
                Points = points
            };

            return response;
        }

        public List<OtherUserPredictionViewModel> GetOthersPredictions(int matchId, int userId)
        {
            var eligibleUsers = dbContext.Users
                .Where(userEligibleSpecification.IsSatisfiedBy)
                .ToList();

            var predictions = dbContext.Predictions
                            .Where(prediction =>
                                    eligibleUsers
                                    .Select(user => user.Id)
                                    .Contains(prediction.UserId) &&
                                    prediction.MatchId == matchId &&
                                    prediction.UserId != userId)
                            .ToList();

            var otherPredictions = predictions.Select(prediction =>
            {
                var user = eligibleUsers.FirstOrDefault(u => u.Id == prediction.UserId);

                var actualMatch = dbContext.Matches.FirstOrDefault(m => m.Id == matchId);
                var actualResult = new MatchResult
                {
                    HomeTeamScore = actualMatch?.HomeTeamScore,
                    AwayTeamScore = actualMatch?.AwayTeamScore,
                };

                var predictedResult = new PredictionRequestDTO
                {
                    HomeTeamScore = prediction?.HomeTeamScore,
                    AwayTeamScore = prediction?.AwayTeamScore
                };

                var pointsObtained = CalculatePoints(actualResult, predictedResult).Points;

                return new OtherUserPredictionViewModel
                {
                    MatchId = matchId,
                    UserName = user?.UserName, 
                    HomeTeamPredictedScore = prediction?.HomeTeamScore.Value,
                    AwayTeamPredictedScore = prediction?.AwayTeamScore.Value,
                    PointsObtained = pointsObtained
                };
            }).ToList();

            return otherPredictions;

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
