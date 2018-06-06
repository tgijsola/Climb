using System;
using System.Net;
using System.Threading.Tasks;
using Climb.Attributes;
using Climb.Data;
using Climb.Responses;
using Climb.Services;
using Climb.Services.ModelServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Climb.API
{
    public class UserApi : BaseApi<UserApi>
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICdnService cdnService;
        private readonly IApplicationUserService applicationUserService;

        public UserApi(ILogger<UserApi> logger, ApplicationDbContext dbContext, ICdnService cdnService, IApplicationUserService applicationUserService)
            : base(logger)
        {
            this.dbContext = dbContext;
            this.cdnService = cdnService;
            this.applicationUserService = applicationUserService;
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