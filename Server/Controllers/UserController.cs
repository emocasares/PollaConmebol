using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollaEngendrilClientHosted.Server.Services;

namespace PollaEngendrilClientHosted.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService usersService;
        public UserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("userid/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            var userId = usersService.GetUserIdByUsername(username);
            return Ok(userId);
        }

        [HttpPost("{username}")]
        public IActionResult CreateUser(string username)
        {
            var userId = usersService.CreateUser(username);
            return Ok(userId);
        }
    }
}
