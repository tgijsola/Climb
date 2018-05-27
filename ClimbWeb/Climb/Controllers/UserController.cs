using System;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Climb.Controllers
{
    public class UserController : Controller
    {
        private readonly IApplicationUserService applicationUserService;

        public UserController(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        [HttpGet("/user/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "User";
            ViewData["Script"] = "user";
            return View("~/Views/Page.cshtml");
        }

        [HttpPost("/api/v1/users/uploadProfilePic")]
        public async Task<IActionResult> UploadProfilePic(int id, IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}