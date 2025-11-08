using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Rentix.Application.Tenants.Commands.Update;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Entities;
using Rentix.Domain.ValueObjects;
using Rentix.Domain.Repositories;
using Xunit;

namespace Rentix.Tests.Unit.Tenants.Update
{
    public class UpdateTenantCommandHandlerTests
    {
        private readonly Mock<ITenantRepository> _tenantRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly UpdateTenantCommandHandler _handler;

        public UpdateTenantCommandHandlerTests()
        {
            _handler = new UpdateTenantCommandHandler(_tenantRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTenant_WhenValid()
        {
            // Arrange
            var tenant = Tenant.Create("John", "Doe", Email.Create("john@doe.com"), Phone.Create("0601020304"));
            tenant.Id = 1;
            _tenantRepositoryMock.Setup(r => r.GetTenantByIdAsync(1)).ReturnsAsync(tenant);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var command = new UpdateTenantCommand(1)
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@smith.com",
                //Phone = "0601020305"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Id.Should().Be(tenant.Id);
            result.FirstName.Should().Be("Jane");
            result.LastName.Should().Be("Smith");
            result.Email.Should().Be("jane@smith.com");
            //result.PhoneNumber.Value.Should().Be("0601020305");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenTenantNotFound()
        {
            // Arrange
            _tenantRepositoryMock.Setup(r => r.GetTenantByIdAsync(1)).ReturnsAsync((Tenant)null);

            var command = new UpdateTenantCommand(1)
            {
                FirstName = "Jane"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Rentix.Application.Exceptions.NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaveChangesFails()
        {
            // Arrange
            var tenant = Tenant.Create("John", "Doe", Email.Create("john@doe.com"), Phone.Create("0601020304"));
            tenant.Id = 1;
            _tenantRepositoryMock.Setup(r => r.GetTenantByIdAsync(1)).ReturnsAsync(tenant);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new Exception("DB error"));

            var command = new UpdateTenantCommand(1)
            {
                FirstName = "Jane"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}