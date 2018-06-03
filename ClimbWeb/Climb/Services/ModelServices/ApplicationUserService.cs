using System;
using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Climb.Extensions;
using Climb.Requests.Account;
using Climb.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Climb.Services.ModelServices
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICdnService cdnService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly ITokenHelper tokenHelper;
        private readonly IUrlUtility urlUtility;

        public ApplicationUserService(ApplicationDbContext dbContext, ICdnService cdnService, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IConfiguration configuration, ITokenHelper tokenHelper, IUrlUtility urlUtility)
        {
            this.dbContext = dbContext;
            this.cdnService = cdnService;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.tokenHelper = tokenHelper;
            this.urlUtility = urlUtility;
        }

        public Task<ApplicationUser> Register(RegisterRequest request)
        {
            throw new NotImplementedException();

        }

        public async Task<string> LogIn(LoginRequest request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);
            if(result.Succeeded)
            {
                var token = tokenHelper.CreateUserToken(configuration.GetSecurityKey(), DateTime.Now.AddMinutes(30), request.Email);
                return token;
            }

            throw new BadRequestException();
        }

        public async Task<string> UploadProfilePic(string userID, IFormFile image)
        {
            if(image == null)
            {
                throw new BadRequestException(nameof(image), "No image uploaded.");
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userID);
            if(user == null)
            {
                throw new NotFoundException(typeof(ApplicationUser), userID);
            }

            dbContext.Update(user);

            if(!string.IsNullOrWhiteSpace(user.ProfilePicKey))
            {
                await cdnService.DeleteImageAsync(user.ProfilePicKey, ClimbImageRules.ProfilePic);
                user.ProfilePicKey = "";
            }

            var imageKey = await cdnService.UploadImageAsync(image, ClimbImageRules.ProfilePic);
            var imageUrl = cdnService.GetImageUrl(imageKey, ClimbImageRules.ProfilePic);

            user.ProfilePicKey = imageKey;
            await dbContext.SaveChangesAsync();

            return imageUrl;
        }
    }
}