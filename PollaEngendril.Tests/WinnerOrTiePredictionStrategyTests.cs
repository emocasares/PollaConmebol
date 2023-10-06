using PollaEngendrilClientHosted.Server.Services.Staregies;
using PollaEngendrilClientHosted.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollaEngendril.Tests
{
    public class WinnerOrTiePredictionStrategyTests
    {
        [Fact]
        public void CalculatePoints_WinnerPrediction_ReturnsThreePoints()
        {
            // Arrange
            var strategy = new WinnerOrTiePredictionStrategy();
            var actualResult = new MatchResult { HomeTeamScore = 3, AwayTeamScore = 1 };
            var predictedResult = new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1 };

            // Act
            var points = strategy.CalculatePoints(actualResult, predictedResult);

            // Assert
            Assert.Equal(3, points); // Winner prediction awards 3 points.
        }

        // Add more test cases for this strategy if needed
    }
}
