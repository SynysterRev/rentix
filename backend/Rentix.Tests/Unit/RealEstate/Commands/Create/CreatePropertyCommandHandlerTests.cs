using FluentAssertions;
using Moq;
using Rentix.Application.RealEstate.Commands.Create;
using Rentix.Application.Exceptions;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Xunit;
using Rentix.Application.RealEstate.DTOs.Addresses;

namespace Rentix.Tests.Unit.RealEstate.Commands.Create
{
    public class CreatePropertyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidAddressId_ShouldCreatePropertyAndReturnPropertyDto()
        {
            // Arrange
            var address = Address.Create("Street", "Postal", "City", "Country", null);
            address.Id = 1;
            var property = Property.Create(
                "Test Property",
                1000,
                500,
                800,
                200,
                PropertyStatus.Available,
                50,
                2,
                address.Id,
                Guid.NewGuid()
            );
            property.Id = 42;

            var addressRepoMock = new Mock<IAddressRepository>();
            addressRepoMock.Setup(r => r.GetByIdAsync(address.Id))
                .ReturnsAsync(address);

            var propertyRepoMock = new Mock<IPropertyRepository>();
            propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>()))
                .ReturnsAsync(property);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var handler = new CreatePropertyCommandHandler(propertyRepoMock.Object, addressRepoMock.Object, unitOfWorkMock.Object);
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
                AddressId = address.Id,
                LandLordId = property.LandlordId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(property.Id);
            result.Name.Should().Be(property.Name);
            result.Address.Should().NotBeNull();
            result.Address.Street.Should().Be(address.Street);
            result.Address.City.Should().Be(address.City);
            result.Address.PostalCode.Should().Be(address.PostalCode);
            result.Address.Country.Should().Be(address.Country);

            addressRepoMock.Verify(r => r.GetByIdAsync(address.Id), Times.Once);
            propertyRepoMock.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentAddressId_ShouldThrowNotFoundException()
        {
            // Arrange
            var addressRepoMock = new Mock<IAddressRepository>();
            addressRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Address?)null);

            var propertyRepoMock = new Mock<IPropertyRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var handler = new CreatePropertyCommandHandler(propertyRepoMock.Object, addressRepoMock.Object, unitOfWorkMock.Object);
            var command = new CreatePropertyCommand
            {
                Name = "Test",
                MaxRent = 1000,
                Deposit = 500,
                RentNoCharges = 800,
                RentCharges = 200,
                PropertyStatus = PropertyStatus.Available,
                Surface = 50,
                NumberRooms = 2,
                AddressId = 999,
                LandLordId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Address with ID 999 not found");
        }

        [Fact]
        public async Task Handle_WithAddressDto_ShouldCreateAddressAndPropertyAndReturnPropertyDto()
        {
            // Arrange
            var address = Address.Create("Street", "Postal", "City", "Country", null);
            address.Id = 2;
            var property = Property.Create(
                "Test Property",
                1000,
                500,
                800,
                200,
                PropertyStatus.Available,
                50,
                2,
                address.Id,
                Guid.NewGuid()
            );
            property.Id = 43;

            var addressRepoMock = new Mock<IAddressRepository>();
            addressRepoMock.Setup(r => r.AddAsync(It.IsAny<Address>()))
                .ReturnsAsync(address);

            var propertyRepoMock = new Mock<IPropertyRepository>();
            propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>()))
                .ReturnsAsync(property);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var handler = new CreatePropertyCommandHandler(propertyRepoMock.Object, addressRepoMock.Object, unitOfWorkMock.Object);
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
                AddressDto = new AddressCreateDto(
                    address.Street, address.City, address.PostalCode, address.Country, address.Complement),
                LandLordId = property.LandlordId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(property.Id);
            result.Name.Should().Be(property.Name);
            result.Address.Should().NotBeNull();
            result.Address.Street.Should().Be(address.Street);
            result.Address.City.Should().Be(address.City);
            result.Address.PostalCode.Should().Be(address.PostalCode);
            result.Address.Country.Should().Be(address.Country);

            addressRepoMock.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
            propertyRepoMock.Verify(r => r.AddAsync(It.IsAny<Property>()), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithoutAddressIdOrDto_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var addressRepoMock = new Mock<IAddressRepository>();
            var propertyRepoMock = new Mock<IPropertyRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var handler = new CreatePropertyCommandHandler(propertyRepoMock.Object, addressRepoMock.Object, unitOfWorkMock.Object);
            var command = new CreatePropertyCommand
            {
                Name = "Test",
                MaxRent = 1000,
                Deposit = 500,
                RentNoCharges = 800,
                RentCharges = 200,
                PropertyStatus = PropertyStatus.Available,
                Surface = 50,
                NumberRooms = 2,
                LandLordId = Guid.NewGuid()
                // No AddressId, no AddressDto
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Address must be resolved before creating a property.");
        }
    }
}
