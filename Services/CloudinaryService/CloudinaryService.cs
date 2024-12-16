using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace PetFeast_Backend2.Services.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService         
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            if(string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret) )
            {
                throw new Exception("Cloudinary configuration is missing or incomplete");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }


        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };
                
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}
