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

namespace Climb.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly ITokenHelper tokenHelper;
        private readonly IUrlUtility urlUtility;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, ITokenHelper tokenHelper, IUrlUtility urlUtility, ILogger<AccountController> logger)
            : base(logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.tokenHelper = tokenHelper;
            this.urlUtility = urlUtility;
        }

        [HttpPost("/api/v1/account/register")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(ApplicationUser))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), "Email or password is not valid.")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if(TryValidateModel(request))
            {
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email
                };
                var result = await userManager.CreateAsync(user, request.Password);
                if(result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = urlUtility.EmailConfirmationLink(Url, user.Id, code, Request.Scheme);
                    await emailSender.SendEmailConfirmationAsync(request.Email, callbackUrl);

                    await signInManager.SignInAsync(user, false);
                    return CodeResultAndLog(HttpStatusCode.Created, user, "User created a new account with password.");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return CodeResultAndLog(HttpStatusCode.BadRequest, "Email or password is not valid.");
        }

        [HttpPost("/api/v1/account/logIn")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LoginResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), "Email or password is incorrect.")]
        public async Task<IActionResult> LogIn(LoginRequest request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);
            if(result.Succeeded)
            {
                var token = tokenHelper.CreateUserToken(configuration.GetSecurityKey(), DateTime.Now.AddMinutes(30), request.Email);
                var response = new LoginResponse(token);

                return CodeResultAndLog(HttpStatusCode.OK, response, "User logged in.");
            }

            return CodeResultAndLog(HttpStatusCode.BadRequest, "Email or password is incorrect.", "User login failed.");
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