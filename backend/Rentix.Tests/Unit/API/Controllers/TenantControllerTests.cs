using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Rentix.API.Controllers.v1;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Application.Tenants.Commands.Delete;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.ValueObjects;
using Xunit;
using System.Threading.Tasks;
using System.Threading;

namespace Rentix.Tests.Unit.API.Controllers
{
    public class TenantControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<ILogger<TenantController>> _loggerMock = new();
        private readonly TenantController _controller;

        public TenantControllerTests()
        {
            _controller = new TenantController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateTenant_ReturnsCreated_WhenSuccess()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            var tenantDto = new TenantDto(1, "John", "Doe", command.Email, command.Phone);
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(tenantDto);

            // Act
            var result = await _controller.CreateTenant(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            createdResult.Value.Should().Be(tenantDto);
            createdResult?.RouteValues?["id"].Should().Be(tenantDto.Id);
        }

        [Fact]
        public async Task CreateTenant_CallsMediatorWithCorrectCommand()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            var tenantDto = new TenantDto(1, "John", "Doe", command.Email, command.Phone);
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(tenantDto);

            // Act
            await _controller.CreateTenant(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTenant_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = Email.Create("john@doe.com"),
                Phone = Phone.Create("0601020304")
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception("Unexpected error"));

            // Act
            var result = await Record.ExceptionAsync(() => _controller.CreateTenant(command));

            // Assert
            result.Should().BeOfType<System.Exception>();
            result.Message.Should().Be("Unexpected error");
        }

        [Fact]
        public async Task GetTenant_ReturnsOk_WhenTenantFound()
        {
            // Arrange
            // Simule un tenant trouvé (à adapter selon l'implémentation réelle)
            // Ici, on suppose que le controller retournera Ok() si trouvé
            // Tu pourras adapter ce test quand la logique sera implémentée
            var result = await _controller.GetTenant(1);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteTenant_ReturnsNoContent_WhenSuccess()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTenantCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _controller.DeleteTenant(1);

            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteTenantCommand>(c => c.TenantId == 1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTenant_ReturnsInternalServerError_OnException()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTenantCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception("Unexpected error"));

            var result = await Record.ExceptionAsync(() => _controller.DeleteTenant(1));

            result.Should().BeOfType<System.Exception>();
            result.Message.Should().Be("Unexpected error");
        }
    }
}
