using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Rentix.Application.Tenants.Commands.Delete;
using Rentix.Domain.Repositories;
using Xunit;

namespace Rentix.Tests.Unit.Tenants.Commands.Delete
{
    public class DeleteTenantCommandHandlerTests
    {
        private readonly Mock<ITenantRepository> _tenantRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DeleteTenantCommandHandler _handler;

        public DeleteTenantCommandHandlerTests()
        {
            _handler = new DeleteTenantCommandHandler(_tenantRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Should_DeleteTenant_When_TenantExists()
        {
            // Arrange
            var command = new DeleteTenantCommand(1);
            _tenantRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _tenantRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowException_When_RepositoryFails()
        {
            // Arrange
            var command = new DeleteTenantCommand(1);
            _tenantRepositoryMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("DB error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
        }
    }
}
