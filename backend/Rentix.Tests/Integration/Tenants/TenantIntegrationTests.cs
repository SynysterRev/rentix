//using System.Net;
//using System.Net.Http.Json;
//using FluentAssertions;
//using Microsoft.Extensions.DependencyInjection;
//using Rentix.Application.Tenants.Commands.Create;
//using Rentix.Application.Tenants.Commands.Update;
//using Rentix.Application.Tenants.DTOs.Tenants;
//using Rentix.Domain.Entities;
//using Rentix.Domain.ValueObjects;
//using Rentix.Infrastructure.Persistence;
//using Rentix.Tests.Integration.Setup;
//using Xunit;

//namespace Rentix.Tests.Integration.Tenants
//{
//    public class TenantIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
//    {
//        private readonly CustomWebApplicationFactory _factory;
//        private readonly HttpClient _client;
//        private readonly Guid _testLandlordId;

//        public TenantIntegrationTests(CustomWebApplicationFactory factory)
//        {
//            _factory = factory;
//            _client = _factory.CreateClient();
//            _testLandlordId = _factory.DefaultUserId;
//        }

//        [Fact]
//        public async Task GetTenant_ReturnsOk_WhenTenantExists()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();

//            // Act
//            var response = await _client.GetAsync($"/api/v1/tenant/{tenantId}");

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsCreated_WhenValidData()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                Email = "john.doe@example.com",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant.Should().NotBeNull();
//            tenant!.FirstName.Should().Be("John");
//            tenant.LastName.Should().Be("Doe");
//            tenant.Email.Should().Be("john.doe@example.com");
//            tenant.PhoneNumber.Should().Be("0601020304");
//            tenant.Id.Should().BeGreaterThan(0);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsBadRequest_WhenInvalidEmail()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                Email = "invalid-email",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsBadRequest_WhenInvalidPhone()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                Email = "john.doe@example.com",
//                Phone = "123" // Too short
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsBadRequest_WhenMissingFirstName()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "",
//                LastName = "Doe",
//                Email = "john.doe@example.com",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsBadRequest_WhenMissingLastName()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "John",
//                LastName = "",
//                Email = "john.doe@example.com",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsBadRequest_WhenEmailIsEmpty()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                Email = "",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task CreateTenant_ReturnsBadRequest_WhenPhoneIsEmpty()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                Email = "john.doe@example.com",
//                Phone = ""
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task UpdateTenant_ReturnsOk_WhenValidData()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Email = "jane.smith@example.com",
//                Phone = "0607080910"
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant.Should().NotBeNull();
//            tenant!.FirstName.Should().Be("Jane");
//            tenant.LastName.Should().Be("Smith");
//            tenant.Email.Should().Be("jane.smith@example.com");
//            tenant.PhoneNumber.Should().Be("0607080910");
//        }

//        [Fact]
//        public async Task UpdateTenant_ReturnsNotFound_WhenTenantDoesNotExist()
//        {
//            // Arrange
//            var command = new UpdateTenantCommand(999999)
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Email = "jane.smith@example.com",
//                Phone = "0607080910"
//            };

//            // Act
//            var response = await PutAsync("/api/v1/tenant/999999", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task UpdateTenant_ReturnsBadRequest_WhenIdMismatch()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId + 1)
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Email = "jane.smith@example.com",
//                Phone = "0607080910"
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task UpdateTenant_ReturnsBadRequest_WhenInvalidEmail()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Email = "invalid-email-format",
//                Phone = "0607080910"
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task UpdateTenant_ReturnsBadRequest_WhenInvalidPhone()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = "Jane",
//                LastName = "Smith",
//                Email = "jane.smith@example.com",
//                Phone = "12" // Too short
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task UpdateTenant_PartialUpdate_OnlyUpdatesProvidedFields()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = "UpdatedFirst",
//                LastName = null,
//                Email = null,
//                Phone = null
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant.Should().NotBeNull();
//            tenant!.FirstName.Should().Be("UpdatedFirst");
//            // Original values should be preserved
//            tenant.LastName.Should().Be("TestLast");
//            tenant.Email.Should().Be("test@example.com");
//            tenant.PhoneNumber.Should().Be("0601020304");
//        }

//        [Fact]
//        public async Task DeleteTenant_ReturnsNoContent_WhenTenantExists()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();

//            // Act
//            var response = await DeleteAsync($"/api/v1/tenant/{tenantId}");

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

//            // Verify tenant is deleted by trying to get it
//            using var scope = _factory.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//            var deletedTenant = await dbContext.Tenants.FindAsync(tenantId);
//            deletedTenant.Should().BeNull();
//        }

//        [Fact]
//        public async Task DeleteTenant_ReturnsNotFound_WhenTenantDoesNotExist()
//        {
//            // Act
//            var response = await DeleteAsync("/api/v1/tenant/999999");

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task TenantWorkflow_CreateUpdateDeleteTenant_Success()
//        {
//            // Create
//            var createCommand = new CreateTenantCommand
//            {
//                FirstName = "Workflow",
//                LastName = "Test",
//                Email = "workflow.test@example.com",
//                Phone = "0611223344"
//            };

//            var createResponse = await PostAsync("/api/v1/tenant", createCommand);
//            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
//            var createdTenant = await createResponse.Content.ReadFromJsonAsync<TenantDto>();
//            createdTenant.Should().NotBeNull();
//            var tenantId = createdTenant!.Id;

//            // Read
//            var getResponse = await _client.GetAsync($"/api/v1/tenant/{tenantId}");
//            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

//            // Update
//            var updateCommand = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = "UpdatedWorkflow",
//                LastName = "UpdatedTest",
//                Email = "updated.workflow@example.com",
//                Phone = "0699887766"
//            };
//            var updateResponse = await PutAsync($"/api/v1/tenant/{tenantId}", updateCommand);
//            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
//            var updatedTenant = await updateResponse.Content.ReadFromJsonAsync<TenantDto>();
//            updatedTenant!.FirstName.Should().Be("UpdatedWorkflow");
//            updatedTenant.Email.Should().Be("updated.workflow@example.com");

//            // Delete
//            var deleteResponse = await DeleteAsync($"/api/v1/tenant/{tenantId}");
//            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

//            // Verify deletion
//            using var scope = _factory.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//            var deletedTenant = await dbContext.Tenants.FindAsync(tenantId);
//            deletedTenant.Should().BeNull();
//        }

//        [Fact]
//        public async Task CreateMultipleTenants_AllPersistCorrectly()
//        {
//            // Arrange & Act
//            var tenant1 = new CreateTenantCommand
//            {
//                FirstName = "First1",
//                LastName = "Last1",
//                Email = "first1@example.com",
//                Phone = "0601020304"
//            };
//            var tenant2 = new CreateTenantCommand
//            {
//                FirstName = "First2",
//                LastName = "Last2",
//                Email = "first2@example.com",
//                Phone = "0605060708"
//            };
//            var tenant3 = new CreateTenantCommand
//            {
//                FirstName = "First3",
//                LastName = "Last3",
//                Email = "first3@example.com",
//                Phone = "0609101112"
//            };

//            var response1 = await PostAsync("/api/v1/tenant", tenant1);
//            var response2 = await PostAsync("/api/v1/tenant", tenant2);
//            var response3 = await PostAsync("/api/v1/tenant", tenant3);

//            // Assert
//            response1.StatusCode.Should().Be(HttpStatusCode.Created);
//            response2.StatusCode.Should().Be(HttpStatusCode.Created);
//            response3.StatusCode.Should().Be(HttpStatusCode.Created);

//            // Verify all were persisted
//            using var scope = _factory.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//            var tenants = dbContext.Tenants.ToList();
//            tenants.Should().HaveCount(3);
//        }

//        [Fact]
//        public async Task CreateTenant_WithSpecialCharactersInName_Success()
//        {
//            // Arrange
//            var command = new CreateTenantCommand
//            {
//                FirstName = "François",
//                LastName = "O'Brien-Smith",
//                Email = "francois.obrien@example.com",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant!.FirstName.Should().Be("François");
//            tenant.LastName.Should().Be("O'Brien-Smith");
//        }

//        [Fact]
//        public async Task CreateTenant_WithLongValidName_Success()
//        {
//            // Arrange
//            var longName = new string('a', 50); // 50 characters
//            var command = new CreateTenantCommand
//            {
//                FirstName = longName,
//                LastName = "Test",
//                Email = "long.name@example.com",
//                Phone = "0601020304"
//            };

//            // Act
//            var response = await PostAsync("/api/v1/tenant", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant!.FirstName.Should().Be(longName);
//        }

//        [Fact]
//        public async Task UpdateTenant_UpdateOnlyEmail_Success()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = null,
//                LastName = null,
//                Email = "newemail@example.com",
//                Phone = null
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant!.Email.Should().Be("newemail@example.com");
//            tenant.FirstName.Should().Be("TestFirst");
//            tenant.LastName.Should().Be("TestLast");
//            tenant.PhoneNumber.Should().Be("0601020304");
//        }

//        [Fact]
//        public async Task UpdateTenant_UpdateOnlyPhone_Success()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();
//            var command = new UpdateTenantCommand(tenantId)
//            {
//                FirstName = null,
//                LastName = null,
//                Email = null,
//                Phone = "0698765432"
//            };

//            // Act
//            var response = await PutAsync($"/api/v1/tenant/{tenantId}", command);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
//            tenant!.PhoneNumber.Should().Be("0698765432");
//            tenant.FirstName.Should().Be("TestFirst");
//            tenant.LastName.Should().Be("TestLast");
//            tenant.Email.Should().Be("test@example.com");
//        }

//        [Fact]
//        public async Task DeleteTenant_MultipleTimes_SecondReturnsNotFound()
//        {
//            // Arrange
//            var tenantId = await SeedSingleTenantAsync();

//            // Act - First delete
//            var firstDeleteResponse = await DeleteAsync($"/api/v1/tenant/{tenantId}");
//            firstDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

//            // Act - Second delete
//            var secondDeleteResponse = await DeleteAsync($"/api/v1/tenant/{tenantId}");

//            // Assert
//            secondDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        // Helper methods
//        private async Task<int> SeedSingleTenantAsync()
//        {
//            using var scope = _factory.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//            var tenant = Tenant.Create(
//                "TestFirst",
//                "TestLast",
//                Email.Create("test@example.com"),
//                Phone.Create("0601020304")
//            );
//            dbContext.Tenants.Add(tenant);
//            await dbContext.SaveChangesAsync();

//            return tenant.Id;
//        }

//        private async Task<HttpResponseMessage> PostAsync<T>(string url, T payload)
//        {
//            return await _client.PostAsJsonAsync(url, payload);
//        }

//        private async Task<HttpResponseMessage> PutAsync<T>(string url, T payload)
//        {
//            return await _client.PutAsJsonAsync(url, payload);
//        }

//        private async Task<HttpResponseMessage> DeleteAsync(string url)
//        {
//            return await _client.DeleteAsync(url);
//        }

//        public async Task InitializeAsync()
//        {
//            await _factory.ResetDatabaseAsync();
//        }

//        public Task DisposeAsync()
//        {
//            return Task.CompletedTask;
//        }
//    }
//}
