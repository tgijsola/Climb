using Climb.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Climb.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("/user/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "User";
            ViewData["Script"] = "user";
            return View("~/Views/Page.cshtml");
        }
    }
}