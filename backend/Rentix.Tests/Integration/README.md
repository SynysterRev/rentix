# Integration Tests

This folder contains integration tests for the Rentix API endpoints.

## Structure

- `Setup/` - Test infrastructure and base classes
  - `CustomWebApplicationFactory.cs` - Configures the test server
  - `IntegrationTestBase.cs` - Base class for all integration tests
- `RealEstate/` - Property API integration tests  
- `Tenants/` - Tenant API integration tests

## Test Coverage

### Property API Tests
- GET /api/v1/property - List all properties (empty and with data)
- GET /api/v1/property/{id} - Get property details (success and not found)
- POST /api/v1/property - Create property (with AddressDto, with AddressId, validation errors)
- PUT /api/v1/property/{id} - Update property (full and partial updates, validation, not found)
- DELETE /api/v1/property/{id} - Delete property (success and not found)
- Complete CRUD workflow test

### Tenant API Tests  
- GET /api/v1/tenant/{id} - Get tenant (success and not found)
- POST /api/v1/tenant - Create tenant (success, validation errors for email/phone/names)
- PUT /api/v1/tenant/{id} - Update tenant (full and partial updates, validation, ID mismatch, not found)
- DELETE /api/v1/tenant/{id} - Delete tenant (success, not found, double delete)
- Complete CRUD workflow test
- Multiple tenant creation test
- Special characters and edge cases

## Current Status

⚠️ **Note**: Integration tests are currently failing due to database provider configuration issues.

The tests are properly structured and comprehensive, covering:
- Success scenarios
- Edge cases (validation errors, not found, ID mismatches)
- Partial updates
- Complete CRUD workflows
- Special input handling

### Known Issue

The WebApplicationFactory is encountering a conflict between the PostgreSQL provider (configured in production code) and the test database provider (SQLite/InMemory). This is a common challenge with ASP.NET Core integration testing when the production code registers EF Core services directly.

### Potential Solutions

1. Modify `DependencyInjection.cs` in Infrastructure to check environment and conditionally register database
2. Use a test-specific appsettings.Testing.json configuration
3. Implement IClassFixture pattern with proper database lifecycle management
4. Use a real test database instance instead of in-memory

## Running Unit Tests

All 107 unit tests pass successfully:

```bash
cd backend
dotnet test --filter "FullyQualifiedName~Unit"
```

## Production Code Changes

Minimal changes were made to production code to enable integration testing:

1. **Rentix.API/Program.cs** - Added `public partial class Program { }` at the end to make it accessible to tests
2. **Rentix.API/Rentix.API.csproj** - Added `<InternalsVisibleTo>Rentix.Tests</InternalsVisibleTo>` to expose internal types

These are standard, minimal changes required for ASP.NET Core integration testing.

## Test Examples

### Success Scenario
```csharp
[Fact]
public async Task CreateTenant_ReturnsCreated_WhenValidData()
{
    var command = new CreateTenantCommand
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        Phone = "0601020304"
    };

    var response = await PostAsync("/api/v1/tenant", command);

    response.StatusCode.Should().Be(HttpStatusCode.Created);
    var tenant = await response.Content.ReadFromJsonAsync<TenantDto>();
    tenant.Should().NotBeNull();
    tenant!.FirstName.Should().Be("John");
}
```

### Validation Error Scenario
```csharp
[Fact]
public async Task CreateTenant_ReturnsBadRequest_WhenInvalidEmail()
{
    var command = new CreateTenantCommand
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "invalid-email",
        Phone = "0601020304"
    };

    var response = await PostAsync("/api/v1/tenant", command);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}
```

### Complete Workflow Test
```csharp
[Fact]
public async Task TenantWorkflow_CreateUpdateDeleteTenant_Success()
{
    // Create
    var createResponse = await PostAsync("/api/v1/tenant", createCommand);
    var tenantId = createdTenant!.Id;

    // Read
    var getResponse = await Client.GetAsync($"/api/v1/tenant/{tenantId}");

    // Update  
    var updateResponse = await PutAsync($"/api/v1/tenant/{tenantId}", updateCommand);

    // Delete
    var deleteResponse = await DeleteAsync($"/api/v1/tenant/{tenantId}");
}
```
