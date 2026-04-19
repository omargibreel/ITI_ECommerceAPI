using Common.Settings;
using Common.Result;
using Ecommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Runtime;



namespace Ecommerce.BLL.Services.Classes
{
    public class PhotoService : IPhotoService
    {
        private readonly ImageSettings _setting;
        public PhotoService(IOptions<ImageSettings> options)
        {
            _setting = options.Value;
        }
        public async Task<Result<string>> UploadAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return Result<string>.ValidationFail("File is empty or null.");

            if (file.Length > _setting.MaxFileSize)
                return Result<string>.ValidationFail($"File size exceeds the maximum limit of {_setting.MaxFileSize / 1024 / 1024} MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_setting.AllowedExtensions.Contains(extension))
                return Result<string>.ValidationFail($"File type {extension} is not allowed. Allowed types: {string.Join(", ", _setting.AllowedExtensions)}.");

            try
            {
                var uploadFolder = Path.Combine(_setting.UploadBasePath, folder);
                Directory.CreateDirectory(uploadFolder);
                var uniqueName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploadFolder, uniqueName);

                await using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Result<string>.Success($"/images/{folder}/{uniqueName}");
            }
            catch (Exception ex)
            {

                return Result<string>.FailOperation($"Upload failed: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(string relativePath) 
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Result.Success();
            try
            {
                var trimmed = relativePath.TrimStart('/').Replace('/',
               Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(_setting.UploadBasePath, trimmed);
                if (File.Exists(fullPath)) File.Delete(fullPath);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.FailOperation($"Delete failed: {ex.Message}");
            }
        }
    }
}
