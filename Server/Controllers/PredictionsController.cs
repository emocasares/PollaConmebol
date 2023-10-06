using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollaEngendrilClientHosted.Server.Services;
using PollaEngendrilClientHosted.Shared;

namespace PollaEngendrilClientHosted.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PredictionsController : ControllerBase
    {
        private readonly IPredictionService predictionService;

        public PredictionsController(IPredictionService predictionService)
        {
            this.predictionService = predictionService;
        }

        [HttpPost("calculate-points")]
        public IActionResult CalculatePoints([FromBody] PredictionRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request.");
            }

            // Assume you have actual match results and a user's predicted match results.
            MatchResult actualResult = GetActualMatchResult();
            var predictedResult = new MatchResult { AwayTeamScore = request.AwayTeamScore, HomeTeamScore = request.HomeTeamScore};

            int points = predictionService.CalculatePoints(actualResult, predictedResult);

            return Ok(new { Points = points });
        }

        private MatchResult GetActualMatchResult()
        {
            // Implement logic to retrieve actual match results from your data source.
            // For simplicity, we'll create a mock result here.
            return new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1 };
        }
    }
}
