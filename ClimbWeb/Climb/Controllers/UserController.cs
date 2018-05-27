using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Extensions;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class UserController : BaseController<UserController>
    {
        private readonly IApplicationUserService applicationUserService;

        public UserController(IApplicationUserService applicationUserService, ILogger<UserController> logger)
            : base(logger)
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
        public async Task<IActionResult> UploadProfilePic(string userID, IFormFile image)
        {
            try
            {
                var imageUrl = await applicationUserService.UploadProfilePic(userID, image);
                return this.CodeResultAndLog(HttpStatusCode.Created, imageUrl, $"Uploaded new profile pic for {userID}.", logger);
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, new {id = userID, image});
            }
        }
    }
}