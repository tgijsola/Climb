﻿using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Responses;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.Controllers
{
    public class UserController : BaseController<UserController>
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IApplicationUserService applicationUserService;
        private readonly ICdnService cdnService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(ApplicationDbContext dbContext, IApplicationUserService applicationUserService, ILogger<UserController> logger, ICdnService cdnService, UserManager<ApplicationUser> userManager)
            : base(logger)
        {
            this.dbContext = dbContext;
            this.applicationUserService = applicationUserService;
            this.cdnService = cdnService;
            this.userManager = userManager;
        }

        [HttpGet("users/home/{userID?}")]
        public async Task<IActionResult> Home(string userID)
        {
            var appUser = await userManager.GetUserAsync(User);
            var id = userID ?? appUser?.Id;

            var user = await dbContext.Users
                .Include(u => u.LeagueUsers).ThenInclude(lu => lu.League).AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            var viewModel = HomeViewModel.Create(user, cdnService);

            return View(viewModel);
        }

        [HttpGet("/api/v1/users/{userID}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(UserDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Could not find user.")]
        public async Task<IActionResult> Get(string userID)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userID);
            if(user == null)
            {
                return CodeResultAndLog(HttpStatusCode.NotFound, $"Could not find user with ID '{userID}'.");
            }

            var response = UserDto.Create(user, cdnService);
            return CodeResult(HttpStatusCode.OK, response);
        }

        [HttpPost("/api/v1/users/uploadProfilePic")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(string), "Profile picture URL.")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), "Couldn't find user.")]
        public async Task<IActionResult> UploadProfilePic(string userID, IFormFile image)
        {
            try
            {
                var imageUrl = await applicationUserService.UploadProfilePic(userID, image);
                return CodeResultAndLog(HttpStatusCode.Created, imageUrl, $"Uploaded new profile pic for {userID}.");
            }
            catch(Exception exception)
            {
                return GetExceptionResult(exception, new {id = userID, image});
            }
        }
    }
}