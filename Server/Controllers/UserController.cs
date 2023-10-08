using Microsoft.AspNetCore.Mvc;

namespace PollaEngendrilClientHosted.Server.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
