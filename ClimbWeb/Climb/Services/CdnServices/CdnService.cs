using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Climb.Services
{
    public abstract class CdnService : ICdnService
    {
        private readonly string root;

        protected CdnService(string root)
        {
            this.root = root;
        }

        public string GetImageUrl(string imageKey, ImageRules rules) => string.IsNullOrWhiteSpace(imageKey) ? rules.MissingUrl : $"{root}/{rules.Folder}/{imageKey}";

        protected abstract Task UploadImageInternalAsync(IFormFile image, string folder, string fileKey);

        public async Task<string> UploadImageAsync(IFormFile image, ImageRules rules)
        {
            if(!IsValid(image, rules))
            {
                throw new ArgumentException($"Image size {image.Length:N0}B exceeds limit {rules.MaxSize:N0}B.");
            }

            EnsureFolder(rules.Folder);

            var fileKey = GenerateFileKey(image);
            await UploadImageInternalAsync(image, rules.Folder, fileKey);
            return fileKey;
        }

        protected abstract void EnsureFolder(string rulesFolder);

        public abstract Task DeleteImageAsync(string fileKey, ImageRules rules);

        private static bool IsValid(IFormFile image, ImageRules rules)
        {
            return image.Length <= rules.MaxSize;
        }

        private static string GenerateFileKey(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = Path.GetInvalidFileNameChars().Aggregate(Path.GetFileNameWithoutExtension(file.FileName), (current, c) => current.Replace(c, '_'));
            fileName = fileName.Replace(".", "");
            var fileKey = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
            return fileKey;
        }
    }
}