using PollaEngendrilClientHosted.Server.Services.ScoringStaregies;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

namespace PollaEngendril.Tests
{
    public class ExactScorePredictionStrategyTests
    {
        [Fact]
        public void CalculatePoints_ExactScorePrediction_ReturnsFivePoints()
        {
            // Arrange
            var strategy = new ExactScorePredictionStrategy();
            var actualResult = new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1 };
            var predictedResult = new PredictionRequestDTO { HomeTeamScore = 2, AwayTeamScore = 1 };

            // Act
            var points = strategy.CalculatePoints(actualResult, predictedResult);

            // Assert
            Assert.Equal(5, points);
        }
    }
}