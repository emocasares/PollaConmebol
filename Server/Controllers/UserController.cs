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

        [HttpGet("userid/{username}/{nickname}")]
        public IActionResult GetUserByUsernameOrNickname(string username, string nickname)
        {
            var userId = usersService.GetUserIdByUsernameOrNickname(username, nickname);
            return Ok(userId);
        }

        [HttpPost("{username}/{nickname}")]
        public IActionResult CreateUser(string username, string nickname)
        {
            var userId = usersService.CreateUser(username, nickname);
            return Ok(userId);
        }
    }
}
