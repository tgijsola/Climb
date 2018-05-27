using System.Threading.Tasks;
using Climb.Data;
using Climb.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Climb.Services.ModelServices
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICdnService cdnService;

        public ApplicationUserService(ApplicationDbContext dbContext, ICdnService cdnService)
        {
            this.dbContext = dbContext;
            this.cdnService = cdnService;
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