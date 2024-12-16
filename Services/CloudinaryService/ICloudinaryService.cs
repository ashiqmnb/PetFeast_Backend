namespace PetFeast_Backend2.Services.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
