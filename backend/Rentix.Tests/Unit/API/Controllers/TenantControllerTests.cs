using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Rentix.API.Controllers.v1;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Application.Tenants.Commands.Delete;
using Rentix.Application.Tenants.Commands.Update;
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
        private readonly Mock<ILogger<TenantsController>> _loggerMock = new();
        private readonly TenantsController _controller;

        public TenantControllerTests()
        {
            _controller = new TenantsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateTenant_ReturnsCreated_WhenSuccess()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Phone = "0601020304"
            };
            var tenantDto = new TenantDto(1, "John", "Doe", command.Email, command.Phone);
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(tenantDto);

            // Act
            var result = await _controller.CreateTenant(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdResult.Value);
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
                Email = "john@doe.com",
                Phone = "0601020304"
            };
            var tenantDto = new TenantDto(1, "John", "Doe", command.Email, command.Phone);
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(tenantDto);

            // Act
            await _controller.CreateTenant(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTenant_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "notanemail",
                Phone = "0601020304"
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ThrowsAsync(new System.ArgumentException("Invalid email format"));

            // Act
            var result = await Record.ExceptionAsync(() => _controller.CreateTenant(command));

            // Assert
            result.Should().BeOfType<System.ArgumentException>();
            result.Message.Should().Be("Invalid email format");
        }

        [Fact]
        public async Task CreateTenant_ReturnsBadRequest_WhenPhoneIsInvalid()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Phone = "notaphone"
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ThrowsAsync(new System.ArgumentException("Invalid phone format"));

            // Act
            var result = await Record.ExceptionAsync(() => _controller.CreateTenant(command));

            // Assert
            result.Should().BeOfType<System.ArgumentException>();
            result.Message.Should().Be("Invalid phone format");
        }

        [Fact]
        public async Task CreateTenant_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var command = new CreateTenantCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Phone = "0601020304"
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
            var result = await _controller.GetTenant(1);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteTenant_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTenantCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTenant(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteTenantCommand>(c => c.TenantId == 1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTenant_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTenantCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception("Unexpected error"));

            // Act
            var result = await Record.ExceptionAsync(() => _controller.DeleteTenant(1));

            // Assert
            result.Should().BeOfType<System.Exception>();
            result.Message.Should().Be("Unexpected error");
        }

        private UpdateTenantCommand GetSampleUpdateCommand(int id = 1)
        {
            return new UpdateTenantCommand(id)
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@smith.com",
                Phone = "0601020305"
            };
        }

        private TenantDto GetSampleUpdatedTenantDto(int id = 1)
        {
            return new TenantDto(id, "Jane", "Smith", "jane@smith.com", "0601020305");
        }

        [Fact]
        public async Task UpdateTenant_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var command = GetSampleUpdateCommand();
            var tenantDto = GetSampleUpdatedTenantDto();
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(tenantDto);

            // Act
            var result = await _controller.UpdateTenant(1, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().Be(tenantDto);
            var returnedDto = Assert.IsType<TenantDto>(okResult.Value);
            returnedDto.FirstName.Should().Be("Jane");
            returnedDto.LastName.Should().Be("Smith");
            returnedDto.Email.Should().Be("jane@smith.com");
            returnedDto.PhoneNumber.Should().Be("0601020305");
        }

        [Fact]
        public async Task UpdateTenant_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var command = GetSampleUpdateCommand(2);

            // Act
            var result = await _controller.UpdateTenant(1, command);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            badRequest.Value.Should().Be("ID mismatch");
        }

        [Fact]
        public async Task UpdateTenant_ThrowsNotFoundException_WhenTenantNotFound()
        {
            // Arrange
            var command = GetSampleUpdateCommand();
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Rentix.Application.Exceptions.NotFoundException("Tenant with ID1 not found"));

            // Act
            var ex = await Record.ExceptionAsync(() => _controller.UpdateTenant(1, command));

            // Assert
            ex.Should().BeOfType<Rentix.Application.Exceptions.NotFoundException>();
            ex.Message.Should().Be("Tenant with ID1 not found");
        }

        [Fact]
        public async Task UpdateTenant_ThrowsException_OnUnhandledError()
        {
            // Arrange
            var command = GetSampleUpdateCommand();
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var ex = await Record.ExceptionAsync(() => _controller.UpdateTenant(1, command));

            // Assert
            ex.Should().BeOfType<Exception>();
            ex.Message.Should().Be("Unexpected error");
        }
    }
}
