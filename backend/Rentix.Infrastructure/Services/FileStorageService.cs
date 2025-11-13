using Rentix.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Rentix.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<byte[]> GetFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            var uploadsPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Documents");

            Directory.CreateDirectory(uploadsPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(outputStream);
            }

            return filePath;
        }
    }
}
