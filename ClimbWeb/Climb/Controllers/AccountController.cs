using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Extensions;
using Climb.Requests.Account;
using Climb.Responses;
using Climb.Services;
using Climb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Climb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly ITokenHelper tokenHelper;
        private readonly IUrlUtility urlUtility;

        public AccountController(SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, ITokenHelper tokenHelper, IUrlUtility urlUtility)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.tokenHelper = tokenHelper;
            this.urlUtility = urlUtility;
        }

        [HttpGet("/account/{*page}")]
        [SwaggerIgnore]
        public IActionResult Index()
        {
            ViewData["Title"] = "Account";
            ViewData["Script"] = "account";
            return View("~/Views/Page.cshtml");
        }

        [HttpPost("/api/v1/account/register")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(ApplicationUser))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), IsNullable = false, Description = "Email or password is not valid.")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if(TryValidateModel(request))
            {
                var user = new ApplicationUser {UserName = request.Email, Email = request.Email};
                var result = await userManager.CreateAsync(user, request.Password);
                if(result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = urlUtility.EmailConfirmationLink(Url, user.Id, code, Request.Scheme);
                    await emailSender.SendEmailConfirmationAsync(request.Email, callbackUrl);

                    await signInManager.SignInAsync(user, false);
                    return this.CodeResult(HttpStatusCode.Created, user);
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return BadRequest("Email or password is not valid.");
        }

        [HttpPost("/api/v1/account/logIn")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.BadRequest, null)]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LoginResponse), IsNullable = false)]
        public async Task<IActionResult> LogIn(LoginRequest request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);
            if(result.Succeeded)
            {
                logger.LogInformation("User logged in.");

                var token = tokenHelper.CreateUserToken(configuration.GetSecurityKey(), DateTime.Now.AddMinutes(30), request.Email);

                return Ok(new LoginResponse(token));
            }

            logger.LogInformation("User login failed.");
            return BadRequest();
        }

        [HttpGet("/api/v1/account/test")]
        [Authorize]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string))]
        public async Task<IActionResult> Test([UserToken] string auth, string userID)
        {
            var authorizedId = await tokenHelper.GetAuthorizedUserID(auth);

            if(userID == authorizedId || string.IsNullOrWhiteSpace(userID))
            {
                return Ok("Authorized!");
            }

            return BadRequest("Not the same user!");
        }

        [HttpPost("/api/v1/account/logOut")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string))]
        public async Task<IActionResult> Logout([UserToken] string auth)
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            return RedirectToPage("/Index");
        }
    }
}