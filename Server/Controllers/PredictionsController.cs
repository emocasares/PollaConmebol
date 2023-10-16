using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollaEngendrilClientHosted.Server.Services;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;

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
        public IActionResult CalculatePoints([FromBody] PredictionRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request.");
            }

            MatchResult actualResult = GetActualMatchResult();
            if (actualResult == null)
            {
                return NotFound();
            }

            PredictionResponseDTO response = predictionService.CalculatePoints(actualResult, request);
            return Ok(response);
        }

        [HttpGet("leaderboard")]
        public IActionResult GetLeaderboard()
        {
            var leaderboard = predictionService.CalculateLeaderboard();
            return Ok(leaderboard);
        }

        [HttpPost("save-predictions")]
        public async Task<IActionResult> SavePredictions([FromBody] PredictionRequestDTO prediction)
        {
            try
            {
                await this.predictionService.SavePrediction(prediction);
                return Ok("La predicción se ha guardado exitosamente.");

            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, "Se ha producido un error al guardar la predicción: " + ex.Message);
            }
        }

        [HttpGet("otherspredictions/{matchId}/{userId}")]
        public IActionResult GetOthersPredictions(int matchId, int userId)
        {
            var othersPredictions = predictionService.GetOthersPredictions(matchId, userId);
            return Ok(othersPredictions);
        }

        private MatchResult GetActualMatchResult()
        {
            // Implement logic to retrieve actual match results from your data source.
            // For simplicity, we'll create a mock result here.
            return new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1 };
        }


    }
}
