using System;
using System.Threading.Tasks;
using Climb.Requests.Account;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IApplicationUserService applicationUserService;

        public AccountController(ILogger<AccountController> logger, IApplicationUserService applicationUserService)
            : base(logger)
        {
            this.applicationUserService = applicationUserService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost("[controller]/LogIn")]
        public async Task<IActionResult> LogInPost(LoginRequest request)
        {
            try
            {
                await applicationUserService.LogIn(request);

                return RedirectToAction("Home", "User");
            }
            catch(Exception exception)
            {
                return new BadRequestResult();
            }
        }
        
        [HttpPost("[controller]/Register")]
        public async Task<IActionResult> RegisterPost(RegisterRequest request)
        {
            try
            {
                await applicationUserService.Register(request);

                return RedirectToAction("Home", "User");
            }
            catch(Exception exception)
            {
                return new BadRequestResult();
            }
        }
    }
}