using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PollaEngendrilClientHosted.Client.Services;
using PollaEngendrilClientHosted.Server.Services;
using PollaEngendrilClientHosted.Shared;
using PollaEngendrilClientHosted.Shared.Models.DTO;

namespace PollaEngendrilClientHosted.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FixtureController : ControllerBase
    {
        private readonly IFixturesService fixturesService;

        public FixtureController(IFixturesService fixturesService)
        {
            this.fixturesService = fixturesService;
        }

        [HttpGet("matches")]
        public IActionResult GetLeaderboard()
        {
            var fixture = this.fixturesService.GetFixture();
            return Ok(fixture);
        }
    }
}
