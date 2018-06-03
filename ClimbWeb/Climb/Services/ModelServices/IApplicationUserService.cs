using System.Threading.Tasks;
using Climb.Data;
using Climb.Requests.Account;
using Microsoft.AspNetCore.Http;

namespace Climb.Services.ModelServices
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> Register(RegisterRequest request);
        Task<string> LogIn(LoginRequest request);
        Task<string> UploadProfilePic(string userID, IFormFile image);
    }
}