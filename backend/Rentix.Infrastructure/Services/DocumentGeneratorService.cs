using Microsoft.Extensions.Hosting;
using MiniSoftware;
using Rentix.Application.Common.Interfaces;
using Rentix.Domain.Entities;

namespace Rentix.Infrastructure.Services
{
    public class DocumentGeneratorService : IDocumentGenerator
    {
        private readonly string _templatePath;

        public DocumentGeneratorService(IHostEnvironment hostEnvironment)
        {
            string contentRoot = hostEnvironment.ContentRootPath;

            _templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "bail_vide.docx");
        }

        public async Task<byte[]> GenerateLeaseAgreementAsync(Lease lease)
        {
            var templateBytes = File.ReadAllBytes(_templatePath);
            var value = new Dictionary<string, object>()
            {
                ["physical_person"] = "☑",
                ["moral_person"] = "☐",
                ["Landlord"] = "Test",
            };

            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    await MiniWord.SaveAsByTemplateAsync(memoryStream, templateBytes, value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur MiniWord: {ex.Message}");
                    throw;
                }
                //await MiniWord.SaveAsByTemplateAsync(memoryStream, templateBytes, value);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ToArray();
            }
        }
    }
}
