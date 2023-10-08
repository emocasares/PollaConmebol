﻿using PollaEngendrilClientHosted.Server.Services.ScoringStaregies;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

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
            var predictedResult = new PredictionRequestDTO { HomeTeamScore = 2, AwayTeamScore = 1 };

            // Act
            var points = strategy.CalculatePoints(actualResult, predictedResult);

            // Assert
            Assert.Equal(3, points); 
        }
    }
}
