using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace WebApi2026.Settings
{
    public class CloudinarySettings
    {
        private readonly Cloudinary _cloudinary;

        public CloudinarySettings(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl?.ToString();
        }
    }
}
