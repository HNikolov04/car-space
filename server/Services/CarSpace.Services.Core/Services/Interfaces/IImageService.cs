using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile imageFile, Guid userId, string subFolder);
    Task<string> SaveImageOrDefaultAsync(IFormFile? imageFile, Guid userId, string subFolder, string fallbackPath);
    Task<string> UpdateImageIfProvidedAsync(IFormFile? newImageFile, string existingImagePath, Guid userId, string subFolder);
    Task DeleteImageAsync(string imagePath);
}
