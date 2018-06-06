using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Requests.Account;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICdnService cdnService;

        public AccountController(ILogger<AccountController> logger, IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager, ICdnService cdnService)
            : base(logger, userManager, dbContext)
        {
            this.applicationUserService = applicationUserService;
            this.signInManager = signInManager;
            this.cdnService = cdnService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost("account/login")]
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

        [HttpPost("account/register")]
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

        [HttpPost("account/logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Home", "Site");
        }

        [HttpGet("account/settings")]
        public async Task<IActionResult> Settings()
        {
            var user = await GetViewUserAsync();

            var viewModel = SettingsViewModel.Create(user, cdnService);
            return View(viewModel);
        }

        [HttpPost("account/updatesettings")]
        public async Task<IActionResult> UpdateSettings(UpdateSettingsRequest request)
        {
            // TODO: Handle errors.
            await applicationUserService.UpdateSettings(null, request.Username, request.ProfilePic);

            return RedirectToAction("Settings");
        }
    }
}