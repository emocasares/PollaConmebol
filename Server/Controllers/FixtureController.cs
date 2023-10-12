using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollaEngendrilClientHosted.Client.Services;
using PollaEngendrilClientHosted.Server.Services;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.Entity;

namespace PollaEngendrilClientHosted.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FixtureController : ControllerBase
    {
        private readonly IFixturesService fixturesService;
        private readonly IPredictionService predictionService;
        private readonly IUsersService usersService;

        public FixtureController(IPredictionService predictionService, IFixturesService fixturesService, IUsersService usersService)
        {
            this.predictionService = predictionService;
            this.fixturesService = fixturesService;
            this.usersService = usersService;
        }

        [HttpGet("matches/{user}")]
        public IActionResult GetFixtures(string user)
        {
            var fixture = this.fixturesService.GetFixture(user);
            return Ok(fixture);
        }

    }
}
