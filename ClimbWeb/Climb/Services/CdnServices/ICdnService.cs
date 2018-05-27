using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Climb.Services
{
    public interface ICdnService
    {
        Task DeleteImageAsync(string fileKey, ImageRules rules);
        string GetImageUrl(string imageKey, ImageRules rules);
        Task<string> UploadImageAsync(IFormFile image, ImageRules rules);
    }
}