using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Climb.Services
{
    public class FileStorageCdn : CdnService
    {
        private readonly string localCdnPath;

        public FileStorageCdn()
            : base(@"\temp\cdn")
        {
            localCdnPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", @"temp\cdn");
        }

        protected override async Task UploadImageInternalAsync(IFormFile image, string folder, string fileKey)
        {
            var folderPath = Path.Combine(localCdnPath, folder);
            var filePath = Path.Combine(folderPath, fileKey);

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
        }

        public override Task DeleteImageAsync(string fileKey, ImageRules rules)
        {
            var folder = rules.Folder;
            var folderPath = Path.Combine(localCdnPath, folder);
            var filePath = Path.Combine(folderPath, fileKey);

            File.Delete(filePath);
            return Task.CompletedTask;
        }
    }
}