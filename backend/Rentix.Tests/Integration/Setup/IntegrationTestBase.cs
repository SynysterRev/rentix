using Microsoft.Extensions.DependencyInjection;
using Rentix.Infrastructure.Persistence;
using System.Net.Http.Json;

namespace Rentix.Tests.Integration.Setup
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly HttpClient Client;
        protected readonly CustomWebApplicationFactory<Program> Factory;

        public IntegrationTestBase()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        protected void CleanDatabase()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            var response = await Client.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            return await Client.PostAsJsonAsync(url, data);
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            return await Client.PutAsJsonAsync(url, data);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await Client.DeleteAsync(url);
        }

        public void Dispose()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }
}
