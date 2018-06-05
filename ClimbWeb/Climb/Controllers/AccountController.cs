using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Requests.Account;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(ILogger<AccountController> logger, IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager)
            : base(logger, userManager, dbContext)
        {
            this.applicationUserService = applicationUserService;
            this.signInManager = signInManager;
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
                await applicationUserService.Register(request, Url, Request.Scheme);

                return RedirectToAction("Home", "User");
            }
            catch(Exception exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpPost("[controller]/logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Home", "Site");
        }
    }
}