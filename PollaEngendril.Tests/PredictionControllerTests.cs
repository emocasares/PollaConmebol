using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using Xunit;

namespace PollaEngendril.Tests
{
    public class PredictionControllerTests : IClassFixture<WebApplicationFactory<PollaEngendrilClientHosted.Server.Program>>
    {
        private readonly WebApplicationFactory<PollaEngendrilClientHosted.Server.Program> _factory;


        public PredictionControllerTests(WebApplicationFactory<PollaEngendrilClientHosted.Server.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CalculatePoints_ReturnsCorrectPoints()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new PredictionRequestDTO
            {
                AwayTeamScore = 1,
                HomeTeamScore = 2,
                MatchId = 3
            };

            // Act
            var response = await client.PostAsync("api/predictions/calculate-points",
                new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.Default, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PredictionResponseDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);

            var expected = new PredictionResponseDTO { Points = 5 };

            if (result != null)
            {
                Assert.Equal(expected.Points, result.Points);
            }
        }
    }
}
