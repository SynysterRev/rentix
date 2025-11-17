using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Rentix.Application.Common.Interfaces;

namespace Rentix.Infrastructure.Services
{
    public class FileStorageOptions
    {
        public string UploadPath { get; set; } = string.Empty;
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadPath;

        public FileStorageService(IOptions<FileStorageOptions> options)
        {
            _uploadPath = options.Value.UploadPath;

            Directory.CreateDirectory(_uploadPath);
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            // For backward compatibility save directly into final location
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);

            using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(outputStream);
            }

            return filePath;
        }

        public Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch
            {
                // swallow exceptions; callers should log if needed
            }

            return Task.CompletedTask;
        }
    }
}
