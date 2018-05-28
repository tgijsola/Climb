using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Climb.Services.ModelServices
{
    public interface IApplicationUserService
    {
        Task<string> UploadProfilePic(string userID, IFormFile image);
    }
}