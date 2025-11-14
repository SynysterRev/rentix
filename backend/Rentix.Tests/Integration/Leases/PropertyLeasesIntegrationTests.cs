using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Rentix.Application.Leases.DTOs;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Entities;
using Rentix.Infrastructure.Persistence;
using Rentix.Tests.Integration.Setup;
using Xunit;

namespace Rentix.Tests.Integration.Leases
{
    public class PropertyLeasesIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public PropertyLeasesIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task<int> SeedSinglePropertyAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var address = Address.Create(
                "Test Street",
                "12345",
                "Test City",
                "Test Country",
                null
            );
            dbContext.Addresses.Add(address);
            await dbContext.SaveChangesAsync();

            var property = Property.Create(
                "Test Property",
                1500m,
                2000m,
                1000m,
                500m,
                PropertyStatus.Available,
                50m,
                2,
                address.Id,
                _factory.DefaultUserId
            );
            dbContext.Properties.Add(property);
            await dbContext.SaveChangesAsync();

            return property.Id;
        }

        private MultipartFormDataContent BuildLeaseMultipart(DateTime start, DateTime end, TenantCreateDto[] tenants, byte[]? fileBytes = null)
        {
            var multipart = new MultipartFormDataContent();
            multipart.Add(new StringContent(start.ToString("o")), "StartDate");
            multipart.Add(new StringContent(end.ToString("o")), "EndDate");
            multipart.Add(new StringContent("1000"), "RentAmount");
            multipart.Add(new StringContent("100"), "ChargesAmount");
            multipart.Add(new StringContent("1000"), "Deposit");
            multipart.Add(new StringContent("true"), "IsActive");
            multipart.Add(new StringContent("Test notes"), "Notes");

            // Add tenants using indexed form field names so model binding from multipart works
            for (int i = 0; i < tenants.Length; i++)
            {
                var t = tenants[i];
                multipart.Add(new StringContent(t.FirstName), $"Tenants[{i}].FirstName");
                multipart.Add(new StringContent(t.LastName), $"Tenants[{i}].LastName");
                multipart.Add(new StringContent(t.Email), $"Tenants[{i}].Email");
                multipart.Add(new StringContent(t.PhoneNumber), $"Tenants[{i}].PhoneNumber");
            }

            if (fileBytes != null)
            {
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
                multipart.Add(fileContent, "LeaseDocument", "lease.pdf");
            }

            return multipart;
        }

        [Fact]
        public async Task CreateLease_ReturnsCreated_WhenValid()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();

            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);

            var tenantList = new[] { new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" } };

            var fileBytes = Encoding.UTF8.GetBytes("dummy pdf content");
            var multipart = BuildLeaseMultipart(start, end, tenantList, fileBytes);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var lease = await response.Content.ReadFromJsonAsync<LeaseDto>();
            lease.Should().NotBeNull();
            lease!.PropertyId.Should().Be(propertyId);
            lease.LeaseDocument.Should().NotBeNull();
            lease.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateLease_PersistsTenantsAndDocument()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();

            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);

            var tenantList = new[] {
                new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" },
                new TenantCreateDto { FirstName = "Jane", LastName = "Smith", Email = "jane@smith.com", PhoneNumber = "0612345678" }
            };

            var fileBytes = Encoding.UTF8.GetBytes("dummy pdf content");
            var multipart = BuildLeaseMultipart(start, end, tenantList, fileBytes);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var lease = await response.Content.ReadFromJsonAsync<LeaseDto>();
            lease.Should().NotBeNull();

            // Verify persisted entities in DB
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var persistedLease = await dbContext.Leases
                .Include(l => l.Tenants)
                .Include(l => l.LeaseDocument)
                .FirstOrDefaultAsync(l => l.Id == lease!.Id);

            persistedLease.Should().NotBeNull();
            persistedLease!.Tenants.Should().HaveCount(2);
            persistedLease.Tenants.Select(t => t.Email.Value).Should().Contain(new[] { "john@doe.com", "jane@smith.com" });

            var persistedDoc = await dbContext.Documents.FirstOrDefaultAsync(d => d.Id == persistedLease.LeaseDocument.Id);
            persistedDoc.Should().NotBeNull();
            persistedDoc!.EntityId.Should().Be(persistedLease.Id);
        }

        [Fact]
        public async Task CreateLease_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = 999999;
            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);
            var tenantList = new[] { new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" } };
            var fileBytes = Encoding.UTF8.GetBytes("dummy pdf content");
            var multipart = BuildLeaseMultipart(start, end, tenantList, fileBytes);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateLease_ReturnsBadRequest_WhenTenantInvalid()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();
            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);
            // invalid email and missing firstname
            var tenantList = new[] { new TenantCreateDto { FirstName = "", LastName = "Doe", Email = "notanemail", PhoneNumber = "" } };
            var fileBytes = Encoding.UTF8.GetBytes("dummy pdf content");
            var multipart = BuildLeaseMultipart(start, end, tenantList, fileBytes);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateLease_ReturnsBadRequest_When_TenantsMissing()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();
            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);
            var tenantList = Array.Empty<TenantCreateDto>();
            var fileBytes = Encoding.UTF8.GetBytes("dummy pdf content");
            var multipart = BuildLeaseMultipart(start, end, tenantList, fileBytes);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateLease_ReturnsBadRequest_WhenFileMissing()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();

            var start = DateTime.UtcNow.Date;
            var end = start.AddYears(1);

            var tenantList = new[] { new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" } };
            var multipart = BuildLeaseMultipart(start, end, tenantList, null);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateLease_ReturnsBadRequest_When_DatesInvalid()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();

            var start = DateTime.UtcNow.Date.AddDays(10);
            var end = DateTime.UtcNow.Date; // end before start

            var tenantList = new[] { new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" } };
            var fileBytes = Encoding.UTF8.GetBytes("dummy pdf content");
            var multipart = BuildLeaseMultipart(start, end, tenantList, fileBytes);

            // Act
            var response = await _client.PostAsync($"/api/v1/properties/{propertyId}/leases", multipart);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
