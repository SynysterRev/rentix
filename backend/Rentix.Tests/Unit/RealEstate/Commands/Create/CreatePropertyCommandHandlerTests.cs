using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Rentix.Application.RealEstate.Commands.Create;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Xunit;
using System;

namespace Rentix.Tests.Unit.RealEstate.Commands.Create
{
    public class CreatePropertyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreatePropertyAndReturnId()
        {
            // Arrange
            var property = Property.Create(
                "Test Property",
                1000,
                500,
                800,
                200,
                PropertyStatus.Available,
                50,
                2,
                1,
                Guid.NewGuid()
            );
            property.Id = 42;
            var repoMock = new Mock<IPropertyRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Property>()))
                .ReturnsAsync(property);
            var handler = new CreatePropertyCommandHandler(repoMock.Object);
            var command = new CreatePropertyCommand
            {
                Name = property.Name,
                MaxRent = property.MaxRent,
                Deposit = property.Deposit,
                RentNoCharges = property.RentNoCharges,
                RentCharges = property.RentCharges,
                PropertyStatus = property.Status,
                Surface = property.Surface,
                NumberRooms = property.NumberRooms,
                AddressId = property.AddressId,
                LandLordId = property.LandlordId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(property.Id);
            repoMock.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
        }
    }
}
