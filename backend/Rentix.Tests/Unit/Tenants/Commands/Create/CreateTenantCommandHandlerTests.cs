using FluentAssertions;
using Moq;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Rentix.Domain.ValueObjects;
using Xunit;

namespace Rentix.Tests.Unit.Tenants.Commands.Create
{
    public class CreateTenantCommandHandlerTests
    {
        private readonly Mock<ITenantRepository> _tenantRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly CreateTenantCommandHandler _handler;

        public CreateTenantCommandHandlerTests()
        {
            _handler = new CreateTenantCommandHandler(_tenantRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        private static Tenant CreateTestTenant(int id = 1, string firstName = "John", string lastName = "Doe", string email = "john@doe.com", string phone = "0601020304")
        {
            var tenant = Tenant.Create(
            firstName,
            lastName,
            Email.Create(email),
            Phone.Create(phone)
            );
            tenant.Id = id;
            return tenant;
        }

        [Fact]
        public async Task Should_CreateTenant_When_FieldsAreValid()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
            };
            var tenant = CreateTestTenant();
            _tenantRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Tenant>())).ReturnsAsync(tenant);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Id.Should().Be(tenant.Id);
            result.FirstName.Should().Be(tenant.FirstName);
            result.LastName.Should().Be(tenant.LastName);
            result.Email.Should().Be(tenant.Email.Value);
            result.PhoneNumber.Should().Be(tenant.Phone.Value);
            _tenantRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tenant>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowException_When_RepositoryFails()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
            };
            _tenantRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Tenant>())).ThrowsAsync(new Exception("DB error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }

        [Fact]
        public async Task Should_ThrowException_When_SaveChangesFails()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                TenantData = new TenantCreateDto { FirstName = "John", LastName = "Doe", Email = "john@doe.com", PhoneNumber = "0601020304" }
            };
            var tenant = CreateTestTenant();
            _tenantRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Tenant>())).ReturnsAsync(tenant);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).ThrowsAsync(new Exception("DB error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}
