using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rentix.Application.RealEstate.Commands.Create.Property;
using Rentix.Application.RealEstate.Commands.Update;
using Rentix.Application.RealEstate.DTOs.Addresses;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;
using Rentix.Infrastructure.Persistence;
using Rentix.Tests.Integration.Setup;
using Xunit;

namespace Rentix.Tests.Integration.RealEstate
{
    public class PropertyIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Guid _testLandlordId;

        public PropertyIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _testLandlordId = _factory.DefaultUserId;
        }

        [Fact]
        public async Task GetProperties_ReturnsEmptyList_WhenNoPropertiesExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/property");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var properties = await response.Content.ReadFromJsonAsync<List<PropertyListDto>>();
            properties.Should().NotBeNull();
            properties.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProperties_ReturnsAllProperties_WhenPropertiesExist()
        {
            // Arrange
            await SeedPropertiesAsync(3);

            // Act
            var response = await _client.GetAsync("/api/v1/property");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var properties = await response.Content.ReadFromJsonAsync<List<PropertyListDto>>();
            properties.Should().NotBeNull();
            properties.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetPropertyDetail_ReturnsProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();

            // Act
            var response = await _client.GetAsync($"/api/v1/property/{propertyId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var property = await response.Content.ReadFromJsonAsync<PropertyDetailDto>();
            property.Should().NotBeNull();
            property!.Id.Should().Be(propertyId);
            property.Name.Should().Be("Test Property");
        }

        [Fact]
        public async Task GetPropertyDetail_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/property/999999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProperty_ReturnsCreated_WhenValidDataWithAddressDto()
        {
            // Arrange
            var command = new CreatePropertyCommand
            {
                Name = "New Property",
                MaxRent = 1500m,
                RentNoCharges = 1200m,
                RentCharges = 300m,
                Deposit = 2400m,
                PropertyStatus = PropertyStatus.Available,
                Surface = 75.5m,
                NumberRooms = 3,
                LandLordId = _testLandlordId,
                AddressDto = new AddressCreateDto(
                    "123 Main St",
                    "Test City",
                    "12345",
                    "Test Country",
                    "Apt 4B"
                )
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/property", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var property = await response.Content.ReadFromJsonAsync<PropertyDetailDto>();
            property.Should().NotBeNull();
            property!.Name.Should().Be("New Property");
            property.RentWithoutCharges.Should().Be(1200m);
            property.RentCharges.Should().Be(300m);
            property.Deposit.Should().Be(2400m);
            property.Surface.Should().Be(75.5m);
            property.NumberRooms.Should().Be(3);
            property.Address.Should().NotBeNull();
            property.Address.Street.Should().Be("123 Main St");
        }

        [Fact]
        public async Task CreateProperty_ReturnsCreated_WhenValidDataWithAddressId()
        {
            // Arrange
            var command = new CreatePropertyCommand
            {
                Name = "New Property with Address ID",
                MaxRent = 1500m,
                RentNoCharges = 1200m,
                RentCharges = 300m,
                Deposit = 2400m,
                PropertyStatus = PropertyStatus.Available,
                Surface = 80m,
                NumberRooms = 4,
                LandLordId = _testLandlordId,
                AddressId = _factory.DefaultAddressId
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/property", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var property = await response.Content.ReadFromJsonAsync<PropertyDetailDto>();
            property.Should().NotBeNull();
            property!.Name.Should().Be("New Property with Address ID");
            property.Address.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateProperty_ReturnsBadRequest_WhenAddressIdDoesNotExist()
        {
            // Arrange
            var command = new CreatePropertyCommand
            {
                Name = "Property with Invalid Address",
                MaxRent = 1500m,
                RentNoCharges = 1200m,
                RentCharges = 300m,
                Deposit = 2400m,
                PropertyStatus = PropertyStatus.Available,
                Surface = 80m,
                NumberRooms = 4,
                LandLordId = _testLandlordId,
                AddressId = 999999
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/property", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProperty_ReturnsBadRequest_WhenMissingBothAddressIdAndDto()
        {
            // Arrange
            var command = new CreatePropertyCommand
            {
                Name = "Property without Address",
                MaxRent = 1500m,
                RentNoCharges = 1200m,
                RentCharges = 300m,
                Deposit = 2400m,
                PropertyStatus = PropertyStatus.Available,
                Surface = 80m,
                NumberRooms = 4,
                LandLordId = _testLandlordId
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/property", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task CreateProperty_ReturnsBadRequest_WhenInvalidData()
        {
            // Arrange - negative rent
            var command = new CreatePropertyCommand
            {
                Name = "Invalid Property",
                MaxRent = -100m,
                RentNoCharges = -50m,
                RentCharges = -50m,
                Deposit = -200m,
                PropertyStatus = PropertyStatus.Available,
                Surface = -10m,
                NumberRooms = 0,
                LandLordId = _testLandlordId,
                AddressDto = new AddressCreateDto(
                    "123 Main St",
                    "Test City",
                    "12345",
                    "Test Country",
                    null
                )
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/property", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateProperty_ReturnsOk_WhenValidData()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();
            var command = new UpdatePropertyCommand(propertyId)
            {
                Name = "Updated Property Name",
                Deposit = 2000m,
                RentNoCharges = 1500m,
                RentCharges = 500m,
                Status = PropertyStatus.Rented,
                Surface = 100m,
                NumberRooms = 5,
                Address = null
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/property/{propertyId}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var property = await response.Content.ReadFromJsonAsync<PropertyDetailDto>();
            property.Should().NotBeNull();
            property!.Name.Should().Be("Updated Property Name");
            property.RentWithoutCharges.Should().Be(1500m);
            property.RentCharges.Should().Be(500m);
            property.NumberRooms.Should().Be(5);
        }

        [Fact]
        public async Task UpdateProperty_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var command = new UpdatePropertyCommand(999999)
            {
                Name = "Updated Property",
                Deposit = 2000m,
                RentNoCharges = 1500m,
                RentCharges = 500m,
                Status = PropertyStatus.Available,
                Surface = 100m,
                NumberRooms = 5,
                Address = null
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/property/999999", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateProperty_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();
            var command = new UpdatePropertyCommand(propertyId + 1)
            {
                Name = "Updated Property",
                Deposit = 2000m,
                RentNoCharges = 1500m,
                RentCharges = 500m,
                Status = PropertyStatus.Available,
                Surface = 100m,
                NumberRooms = 5,
                Address = null
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/property/{propertyId}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateProperty_PartialUpdate_OnlyUpdatesProvidedFields()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();
            var command = new UpdatePropertyCommand(propertyId)
            {
                Name = "Partially Updated",
                Deposit = null,
                RentNoCharges = null,
                RentCharges = null,
                Status = null,
                Surface = null,
                NumberRooms = null,
                Address = null
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/v1/property/{propertyId}", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var property = await response.Content.ReadFromJsonAsync<PropertyDetailDto>();
            property.Should().NotBeNull();
            property!.Name.Should().Be("Partially Updated");
            // Original values should be preserved
            property.RentWithoutCharges.Should().Be(1000m);
            property.NumberRooms.Should().Be(2);
        }

        [Fact]
        public async Task DeleteProperty_ReturnsNoContent_WhenPropertyExists()
        {
            // Arrange
            var propertyId = await SeedSinglePropertyAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/v1/property/{propertyId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verify property is deleted
            var getResponse = await _client.GetAsync($"/api/v1/property/{propertyId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteProperty_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/property/999999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PropertyWorkflow_CreateUpdateDeleteProperty_Success()
        {
            // Create
            var createCommand = new CreatePropertyCommand
            {
                Name = "Workflow Test Property",
                MaxRent = 2000m,
                RentNoCharges = 1500m,
                RentCharges = 500m,
                Deposit = 3000m,
                PropertyStatus = PropertyStatus.Available,
                Surface = 90m,
                NumberRooms = 4,
                LandLordId = _testLandlordId,
                AddressDto = new AddressCreateDto(
                    "456 Oak Ave",
                    "Workflow City",
                    "54321",
                    "Test Country",
                    null
                )
            };

            var createResponse = await _client.PostAsJsonAsync("/api/v1/property", createCommand);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdProperty = await createResponse.Content.ReadFromJsonAsync<PropertyDetailDto>();
            createdProperty.Should().NotBeNull();
            var propertyId = createdProperty!.Id;

            // Read
            var getResponse = await _client.GetAsync($"/api/v1/property/{propertyId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Update
            var updateCommand = new UpdatePropertyCommand(propertyId)
            {
                Name = "Updated Workflow Property",
                Deposit = null,
                RentNoCharges = null,
                RentCharges = null,
                Status = PropertyStatus.Rented,
                Surface = null,
                NumberRooms = null,
                Address = null
            };
            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/property/{propertyId}", updateCommand);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedProperty = await updateResponse.Content.ReadFromJsonAsync<PropertyDetailDto>();
            updatedProperty!.Name.Should().Be("Updated Workflow Property");
            updatedProperty.PropertyStatus.Should().Be(PropertyStatus.Rented);

            // Delete
            var deleteResponse = await _client.DeleteAsync($"/api/v1/property/{propertyId}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verify deletion
            var verifyResponse = await _client.GetAsync($"/api/v1/property/{propertyId}");
            verifyResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // Helper methods
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
                _testLandlordId
            );
            dbContext.Properties.Add(property);
            await dbContext.SaveChangesAsync();

            return property.Id;
        }

        private async Task SeedPropertiesAsync(int count)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            for (int i = 0; i < count; i++)
            {
                var address = Address.Create(
                    $"Test Street {i}",
                    $"1234{i}",
                    $"Test City {i}",
                    "Test Country",
                    null
                );
                dbContext.Addresses.Add(address);
                await dbContext.SaveChangesAsync();

                var property = Property.Create(
                    $"Test Property {i}",
                    1500m + (i * 100),
                    2000m + (i * 100),
                    1000m + (i * 100),
                    500m,
                    PropertyStatus.Available,
                    50m + (i * 10),
                    2 + i,
                    address.Id,
                    _testLandlordId
                );
                dbContext.Properties.Add(property);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task<int> SeedAddressAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var address = Address.Create(
                "Seeded Street",
                "99999",
                "Seeded City",
                "Seeded Country",
                null
            );
            dbContext.Addresses.Add(address);
            await dbContext.SaveChangesAsync();

            return address.Id;
        }

        public async Task InitializeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
