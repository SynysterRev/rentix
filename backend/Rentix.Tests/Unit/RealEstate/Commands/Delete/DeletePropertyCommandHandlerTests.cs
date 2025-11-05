using Xunit;
using Moq;
using Rentix.Application.RealEstate.Commands.Delete;
using Rentix.Domain.Repositories;
using Rentix.Application.Exceptions;

public class DeletePropertyCommandHandlerTests
{
    private readonly Mock<IPropertyRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public DeletePropertyCommandHandlerTests()
    {
        _repoMock = new Mock<IPropertyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    private DeletePropertyCommandHandler CreateHandler()
    {
        return new DeletePropertyCommandHandler(_repoMock.Object, _unitOfWorkMock.Object);
    }

    private void SetupDeleteAsync(int id, bool result)
    {
        _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(result);
    }

    [Fact]
    public async Task Handle_DeletesProperty_WhenExists()
    {
        // Arrange
        SetupDeleteAsync(1, true);
        var handler = CreateHandler();

        // Act
        await handler.Handle(new DeletePropertyCommand(1), CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenNotExists()
    {
        // Arrange
        SetupDeleteAsync(2, false);
        var handler = CreateHandler();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new DeletePropertyCommand(2), CancellationToken.None));
    }
}