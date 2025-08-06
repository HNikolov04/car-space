using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Services;

public class ImageService : IImageService
{
    // Temporary like this
    private const string WEB_ROOT_PATH = "D:\\Work And Online School\\Work\\Projects\\car-space\\server\\WebApi\\CarSpace.WebApi\\wwwroot";
    private const string USER_CONTENT_ROOT = "user-content";

    public async Task<string> SaveImageAsync(IFormFile imageFile, Guid userId, string subFolder)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ValidationAppException("No image file was uploaded.");
        }

        try
        {
            var uploadsFolder = Path.Combine(WEB_ROOT_PATH, USER_CONTENT_ROOT, subFolder);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var extension = Path.GetExtension(imageFile.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ValidationAppException("Image file must have a valid extension.");
            }

            var fileName = $"{userId}_{Path.GetFileNameWithoutExtension(imageFile.FileName)}_{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/{USER_CONTENT_ROOT}/{subFolder}/{fileName}".Replace('\\', '/');
        }
        catch (Exception ex)
        {
            throw new ImageProcessingException("Failed to save the image.", ex);
        }
    }

    public async Task<string> SaveImageOrDefaultAsync(IFormFile? imageFile, Guid userId, string subFolder, string fallbackPath)
    {
        return imageFile == null
            ? fallbackPath
            : await SaveImageAsync(imageFile, userId, subFolder);
    }

    public async Task<string> UpdateImageIfProvidedAsync(IFormFile? newImageFile, string existingImagePath, Guid userId, string subFolder)
    {
        if (newImageFile == null)
        {
            return existingImagePath;
        }

        var newImagePath = await SaveImageAsync(newImageFile, userId, subFolder);
        await DeleteImageAsync(existingImagePath);
        return newImagePath;
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || imagePath.Contains("Default", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        try
        {
            var relativePath = imagePath.TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            var fullPath = Path.Combine(WEB_ROOT_PATH, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            throw new ImageProcessingException($"Failed to delete image: {imagePath}", ex);
        }

        await Task.CompletedTask;
    }
}
